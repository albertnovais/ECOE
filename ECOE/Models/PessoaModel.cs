using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECOE.Models
{
    public class PessoaModel
    {
        ECOEEntities bd = new ECOEEntities();

        public List<string> Search(string name)
        {
            return bd.Pessoa.Where(x => x.Nome.StartsWith(name) && x.AcessoId!= 2).Select(x => x.Nome).ToList();
        }

      
    }
}