using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendasLux.Application.DTOs;
using VendasLux.Application.Interfaces;
using VendasLux.Domain.Entities;
using VendasLux.Domain.Interfaces;

namespace VendasLux.Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public ProdutoResponse Adicionar(ProdutoRequest request)
        {
            // Transforma o DTO que veio da API na Entidade de Domínio
            var produto = new Produto(request.Nome, request.Preco);

            // Salva usando o repositório (lista em memória na Infraestrutura)
            _produtoRepository.Adicionar(produto);

            // Retorna a resposta para a API
            return new ProdutoResponse(produto.Id, produto.Nome, produto.Preco);
        }

        public ProdutoResponse ObterPorId(Guid id)
        {
            var produto = _produtoRepository.ObterPorId(id);
            if (produto == null) return null;

            return new ProdutoResponse(produto.Id, produto.Nome, produto.Preco);
        }

        public IEnumerable<ProdutoResponse> ObterTodos()
        {
            var produtos = _produtoRepository.ObterTodos();
            return produtos.Select(p => new ProdutoResponse(p.Id, p.Nome, p.Preco));
        }
    }
}
