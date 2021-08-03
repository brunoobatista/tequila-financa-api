using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Tequila.Models.DTOs;
using Tequila.Models.Enum;
using Tequila.Models.Interfaces;

namespace Tequila.Models
{
    [Table("despesa")]
    public class Despesa : IEntity
    {
        [Key, Column("id")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Necessário informar a carteira"), Column("carteira_id")]
        public long CarteiraId { get; set; }
        public Carteira Carteira { get; set; }
        
        [Required(ErrorMessage = "Necessário informar o usuário"), Column("usuario_id")]
        public long UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        
        [Column("despesasfixas_id")]
        public long? DespesasFixasId { get; set; }
        public DespesasFixas DespesasFixas { get; set; }

        [Required(ErrorMessage = "Necessário descrisão"), Column("descricao")]
        public string Descricao { get; set; }
        
        [Column("tipo_id")]
        public int TipoId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("status_id")]
        public int StatusId { get; set; }
        
        [ForeignKey("StatusId")]
        public Status Status { get; set; }
        
        [Column("valor")]
        public decimal? Valor { get; set; }
        
        [Column("valor_previsto")]
        public decimal? ValorPrevisto { get; set; }
        
        [Column("parcela_atual")]
        public int? ParcelaAtual { get; set; }
        
        [Column("total_parcelas")]
        public int? TotalParcelas { get; set; }
        
        [Column("data_vencimento")]
        public DateTime? DataVencimento { get; set; }

        [JsonIgnore]
        [Column("ativo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Ativo { get; set; }
        
        [Column("alterado_em")]
        public DateTime? AlteradoEm { get; set; }
        
        [Column("criado_em")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? CriadoEm { get; set; }

        public static Despesa mapperDespesaVariavel(long userId, long carteiraId, DespesaVariavelDTO despesaVariavelDto)
        {
            Despesa despesa = new Despesa();
            despesa.Id = despesaVariavelDto.Id ?? default(long);
            despesa.Descricao = despesaVariavelDto.Descricao;
            despesa.Valor = despesaVariavelDto.Valor;
            despesa.CarteiraId = carteiraId;
            despesa.UsuarioId = userId;
            despesa.TipoId = (int)TIPO.VARIAVEL;
            despesa.StatusId = (int)STATUSDESPESA.FIXADO;

            return despesa;
        }
    }
}