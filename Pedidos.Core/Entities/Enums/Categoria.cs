﻿using System.Text.Json.Serialization;

namespace Pedidos.Core.Entities.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Categoria
    {
        Lanche = 1,
        Acompanhamento = 2,
        Bebida = 3,
        Sobremesa = 4
    }
}
