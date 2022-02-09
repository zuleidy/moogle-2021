using System.Text.RegularExpressions;

namespace MoogleEngine;


public static class Moogle
{

    //testt
    //test2
    public static SearchResult Query(string query)
    {
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
        // string[] queries = query.Split(" ");
        List<string> queries = DividePalabra(query);

        //recorrer cada uno de los documentos
        foreach (var file in files)
        {
            //cada documento va a tener una lista de palabras 
            List<Word> listWord = new List<Word>();

            //leo el documento y obtengo un string con el contenido del mismo 
            string contenido = File.ReadAllText(file);

            //recorro cada una de las palabras a buscar
            foreach (var palabra in queries)
            {
                //creo una palabra por cada palabra de la query con valor inicial del tf en 0
                Word w = new Word(palabra, 0);


                //creo una expression regular con la palagra a buscar y busco todas las veces q esa palabra se repite en el texto 
                /*Regex nuevo = new Regex(palabra.ToLower());
                var result = nuevo.Matches(contenido.ToLower());
                //si se repite mas de una vez calculo el tf
                if (result.Count > 0)
                {
                    w.Tf += result.Count;
                }*/

                int result = Match(palabra.ToLower(), contenido.ToLower());
                if (result > 0)
                {
                    w.Tf += result;
                }

                //adiciono la palabra a la lista de palabras a buscar en el texto
                listWord.Add(w);

            }
            int score = 0;
            //calculo el scrore del documento, q no es mas q la suma de los tf de todas las palabras
            if (listWord.Count > 0)
            {
                foreach (var word in listWord)
                {
                    score += word.Tf;
                }

            }
            if (score>0)
            {
                //obtengo el nombre del documento
                string name = Path.GetFileName(file).Split('.')[0];
                //creo un searchitem que es el obejto q se usa para mostrar la lista de obejtos encontrados en el buscador
                items.Add(new SearchItem(name + score.ToString(), contenido.Substring(0, 5), score));//contenido.IndexOf(queries[0])+100
            }

            //ordeno esos items de mayor a menor
            items = OrdenarDesc(items);
        }
        return new SearchResult(items, query);
    }

    //Este metodo divide las palabras de una frase por espacio
    public static List<string> DividePalabra(string query)
    {
        List<string> result = new List<string>();
        int indice = 0;
        for (int i = 0; i < query.Length; i++)
        {
            //cuando enciunetra un espacio voy a coger todo lo q hay desde el indice hasta el espacio 
            if (query[i] == ' ')
            {
                result.Add(query.Substring(indice, i - indice));
                //el indice para la proxima palabra va a ser una posicion despues del espacio
                indice = i + 1;
            }
        }
        //si aun el indice no es el fin de la cadena es q me falta la ultima palabra
        if (indice< query.Length)
            result.Add(query.Substring(indice, query.Length - indice));
        return result;
    }

    //este metod me devuelve la cantidad de coincidencias (relevancis) de una palabra
    public static int Match(string palabra, string contenido)
    {
        try
        {
            //contador de la cantidad de coincidencias
            int cont = 0;
            for (int i = 0; i < contenido.Length; i++)
            {
                //bandera que indica si se encontro la palabra
                bool result = true;

                for (int j = 0; j < palabra.Length; j++)
                {
                    //si cno coniciden las letras hago un break para q incremente la i de contendido
                    if (contenido[i] != palabra[j])
                    {
                        result = false;
                        break;
                    }
                    //si coinciden las letras tengo q incrementar la i para moverme por las dos listas
                    if (i<contenido.Length-1)
                    {
                        i++;
                    }
                 
                }
                if (result == true)
                    cont++;

            }
            return cont;

        }
        catch (Exception ex)
        {

            throw;
        }
     
    }
    //este metodo ordena descendentemente
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
