using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Tequila.Models
{
    [Table("usuario")]
    public class Usuario
    {

        public Usuario(/*ILazyLoader lazyLoader*/)
        {
            //LazyLoader = lazyLoader;
        }

        //private ILazyLoader LazyLoader { get; set; }

        [Key, Column("id")]
        public long Id { get; set; }

        public virtual Endereco Endereco { get; set; }

        //private Endereco _endereco;
        //public Endereco Endereco 
        //{
        //    get => LazyLoader.Load(this, ref _endereco);
        //    set => _endereco = value;
        //}

        

        [Column("email")] 
        public string Email { get; set; }

        [Column("nome")] 
         public string Nome { get; set; }

        [JsonIgnore]
        [Column("senha")]
        public string Senha { get; set; }

        [Column("avatar")]
        public string Avatar { get; set; }

        [Column("cpf_cnpj")]
        public string CpfCnpj { get; set; }

        [Column("renda")]
        public decimal Renda { get; set; }

        [Column("tipo_renda")]
        public string TipoRenda { get; set; }

        [Column("criado_em")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CriadoEm { get; set; }

        [Column("alterado_em")]
        public DateTime? AlteradoEm { get; set; }

        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ativo")] public int Ativo { get; set; }
    }
}