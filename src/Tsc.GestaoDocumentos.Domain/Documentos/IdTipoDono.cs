﻿using DddBase.Base;

namespace Tsc.GestaoDocumentos.Domain.Documentos
{
    public record IdTipoDono : IdEntidadeBase<Guid>
    {
        public IdTipoDono(Guid valor) : base(valor)
        {
        }
        public static IdTipoDono CriarNovo()
        {
            return new IdTipoDono(Guid.NewGuid());
        }
    }
}