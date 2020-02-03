using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class Item
    {
        public enum EType
        {
            Mask,
            Grandma,
            Newspaper
        }

        public String Name
        {
            get
            {
                return Type.ToString("g");
            }
        }
        public EType Type { get; private set; }

        public Item(EType InType)
        {            
            Type = InType;
        }
    }
