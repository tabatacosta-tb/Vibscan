using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendasLux.Domain.Entities;
using VendasLux.Domain.Interfaces;

namespace VendasLux.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        // Simulando a tabela de Produtos
        private static readonly List<Produto> _produtos = new();

        public void Adicionar(Produto produto)
        {
            _produtos.Add(produto);
        }

        public Produto ObterPorNome(string nome)
        {
            return _produtos.FirstOrDefault(p => p.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
        }

        public Produto ObterPorId(Guid id)
        {
            return _produtos.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Produto> ObterTodos()
        {
            return _produtos;
        }

        public void Atualizar(Produto produto)
        {
            var produtoExistente = ObterPorId(produto.Id);
            if (produtoExistente != null)
            {
                produtoExistente.Atualizar(produto.Nome, produto.Preco);
            }
        }

        public void Remover(Guid id)
        {
            var produto = ObterPorId(id);
            if (produto != null)
            {
                _produtos.Remove(produto);
            }
        }
    }
}
