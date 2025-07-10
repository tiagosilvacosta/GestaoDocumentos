using AutoMapper;
using Tsc.GestaoDocumentos.Application.Documentos;
using Tsc.GestaoDocumentos.Application.Organizacoes;
using Tsc.GestaoDocumentos.Application.Usuarios;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Application.Mappings;

public class DomainToDtoProfile : Profile
{
    public DomainToDtoProfile()
    {
        CreateMap<Organizacao, OrganizacaoDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<Usuario, UsuarioDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Perfil, opt => opt.MapFrom(src => src.Perfil.ToString()));

        CreateMap<TipoDono, TipoDonoDto>()
            .ForMember(dest => dest.TiposDocumentoVinculados, opt => opt.MapFrom(src => 
                src.TiposDocumentoVinculados.Select(x => x.TipoDocumento)));

        CreateMap<TipoDocumento, TipoDocumentoDto>()
            .ForMember(dest => dest.TiposDonoVinculados, opt => opt.MapFrom(src => 
                src.TiposDonoVinculados.Select(x => x.TipoDono)));

        CreateMap<DonoDocumento, DonoDocumentoDto>()
            .ForMember(dest => dest.TipoDonoNome, opt => opt.MapFrom(src => src.TipoDono.Nome))
            .ForMember(dest => dest.DocumentosVinculados, opt => opt.MapFrom(src => 
                src.DocumentosVinculados.Select(x => x.Documento)));

        CreateMap<Documento, DocumentoDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.TipoDocumentoNome, opt => opt.MapFrom(src => src.TipoDocumento.Nome))
            .ForMember(dest => dest.TamanhoFormatado, opt => opt.MapFrom(src => src.ObterTamanhoFormatado()))
            .ForMember(dest => dest.DonosVinculados, opt => opt.MapFrom(src => 
                src.DonosVinculados.Select(x => x.DonoDocumento)));
    }
}
