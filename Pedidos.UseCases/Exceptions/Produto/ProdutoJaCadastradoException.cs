﻿namespace Pedidos.UseCases.Exceptions.Produto
{
    public class ProdutoJaCadastradoException : Exception
    {
        public ProdutoJaCadastradoException()
        {
        }

        public ProdutoJaCadastradoException(string message)
            : base(message)
        {
        }

        public ProdutoJaCadastradoException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
