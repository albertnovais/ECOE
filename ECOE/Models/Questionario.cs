using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECOE.Models
{
    public class Questionario
    {
        ECOEEntities bd = new ECOEEntities();

        public class resposta
        {
            int questaoId;
            int radio;
        }


        public class RespostasLista
        {
            public List<resposta> respostas { set; get; }
            public List<int> questaoId { set; get; }
            public int Radio1 { get; set; }
            public int Radio2 { get; set; }
            public int Radio3 { get; set; }
            public int Radio4 { get; set; }
            public int Radio5 { get; set; }
            public int Radio6 { get; set; }
            public int Radio7 { get; set; }
            public int Radio8 { get; set; }
            public int Radio9 { get; set; }
            public int Radio10 { get; set; }
            public int Radio11 { get; set; }
            public int Radio12 { get; set; }
            public int Radio13 { get; set; }
            public int Radio14 { get; set; }
            public int Radio15 { get; set; }
            public int Radio16 { get; set; }
            public int Radio17 { get; set; }
            public int Radio18 { get; set; }
            public int Radio19 { get; set; }
            public int Radio20 { get; set; }

           

        }

    }
}