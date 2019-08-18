using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECOE.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations; 
    public class Validacao
    {

        public int PessoaId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public int PessoaCadastrou { get; set; }
        public Nullable<int> RA { get; set; }
        public int AcessoId { get; set; }
        public int StatusId { get; set; }


    }
}