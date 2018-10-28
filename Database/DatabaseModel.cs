using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


   public class DatabaseModel : Dictionary<String,Object>
    {
        String field;
        Object value;
    
        public DatabaseModel()
        {

        }
        public void Bulk_Add(String[] Field, Object[] Value)
        {
            for (int i = 0; i < Field.Length; i++)
            {
                this.Add(Field[i], Value[i]);
            }
        }
        

        public string Field
        {
            get
            {
                return field;
            }

            set
            {
                field = value;
            }
        }

        public object Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }
    }

