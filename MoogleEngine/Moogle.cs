using System.Text.RegularExpressions;

namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query) {
        // Modifique este método para responder a la búsqueda

        /*SearchItem[] items = new SearchItem[3] {
            new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.9f),
            new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.5f),
            new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.1f),
        };
*/
        //voy a trabajar con listas en lugar de arreglo, es mas comodo
        List<SearchItem> items = new List<SearchItem>();

        //ruta donde van a estar todos los ficheros 
        string rutaArchivo = @"D:\";

        //arreglo con el texto de cada uno de los documentos
        string[] files = Directory.GetFiles(rutaArchivo, "*.txt");

        //el query q es lo q se pone en el campo del buscador, pueden ser una o varias palabras, y las voy a separar por el espacio
        string[] queries = query.Split(" ");

        //recorrer cada uno de los documentos
        foreach (var file in files)
        {
            //cada documento va a tener una lista de palabras 
            List<Word> listWord = new List<Word>();

            //recorro cada una de las palabras a buscar
            foreach (var palabra in queries)
        {
                //creo una palabra por cada palabra de la query con valor inicial del tf en 0
            Word w = new Word(palabra, 0 );

                //leo el documento y obtengo un string con el contenido del mismo 
                string contenido = File.ReadAllText(file);
               
                //creo una expression regular con la palagra a buscar y busco todas las veces q esa palabra se repite en el texto 
                Regex nuevo = new Regex(palabra.ToLower());
                var result = nuevo.Matches(contenido.ToLower());

                //si se repite mas de una vez calculo el tf
                if (result.Count > 0)
                {
                    w.Tf += result.Count;
                }

                //adiciono la palabra a la lista d epalabras a buscar en el texto
                listWord.Add(w);

            }
            int score = 0;

            //calculo el scrore del documento, q no es mas q la suma de los tf de todas las palabras
            if (listWord.Count>0)
            {
                foreach (var word in listWord)
                {
                    score += word.Tf;
                }

            }
          //obtengo el nombre del documento
          string name = Path.GetFileName(file).Split('.')[0];
            //creo un searchitem que es el obejto q se usa para mostrar la lista de obejtos encontrados en el buscador
            items.Add(new SearchItem(name, "Lorem ipsum dolor sit amet" + score, score));

            //ordeno esos items de mayor a menor
            items = OrdenarDesc(items);
        }
       

        return new SearchResult(items, query);
    }
    public static List<SearchItem> OrdenarDesc(List<SearchItem> list)
    {
        float mayor = 0;
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = i; j < list.Count; j++)
            {
                if (list[i].Score < list[j].Score)
                {
                    SearchItem temp;
                    temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }

            }

        }
        return list;
    }

}
