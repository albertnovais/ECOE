//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ECOE.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Turma
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Turma()
        {
            this.Avaliacoes = new HashSet<Avaliacoes>();
            this.TurmaPessoa = new HashSet<TurmaPessoa>();
        }
    
        public int TurmaId { get; set; }
        public string Nome { get; set; }
        public int CursoId { get; set; }
        public int StatusId { get; set; }
        public int PessoaId { get; set; }
        public string Turno { get; set; }
        public Nullable<int> Ano { get; set; }
        public Nullable<int> Periodo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Avaliacoes> Avaliacoes { get; set; }
        public virtual Curso Curso { get; set; }
        public virtual Status Status { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TurmaPessoa> TurmaPessoa { get; set; }
        public virtual Pessoa Pessoa { get; set; }
    }
}
