using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pedidos.UseCases.Exceptions.Produto
{
    public class AtualizaProdutoException : Exception
    {
        public AtualizaProdutoException()
        {
        }

        public AtualizaProdutoException(string message)
            : base(message)
        {
        }

        public AtualizaProdutoException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
