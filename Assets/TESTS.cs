using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;

public class TESTS : MonoBehaviour {

	public void Start()
    {/*
        UniteTests toTest = new UniteTests();
        List<Result> resultats = new List<Result>();
        List<Conversation> list = new List<Conversation>();
        for (int i = 1; i < 6; i++)
        {
            Conversation c = new Conversation();
            c.Add(new Phrase("normal", "[array:AvisEquipage" + i + "]{0:d} : {1:d}[endarray]"));
            list.Add(c);
        }
        string ap = "";
        foreach (Phrase p in list[0])
        {
            ap += p.Texte + "/status:" + p.temp;
        }
        Debug.Log(ap);

        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list[i].Count; j++)
            {
                toTest.currentPhrase = j;
                resultats.Add(new Result(toTest.ParseText(list[i]), list[i], j,i+1));
            }
            toTest.RESET();
        }
        
        foreach (Result item in resultats)
        {
            int i = 0;
            foreach (Phrase p in item.conv)
            {
                i++;
                Debug.Log(i+p.Texte+"/status:"+p.temp);
            }
            Debug.Log(item.txt+":::FOR PHRASE :"+item.phrase+"& nombre d'affichages = "+ item.nbAff);
        }*/
    }
	
}

public class Result
{
    public string txt;
    public Conversation conv;
    public int phrase;
    public int nbAff;
    public Result(string t,Conversation c,int p,int aff)
    {
        txt = t;
        conv = c;
        phrase = p;
        nbAff = aff;
    }
}

public class UniteTests
{
    public int multipleDisplay = 1;
    public int currentPhrase = 0;
    public void RESET()
    {
        multipleDisplay = 1;
        currentPhrase = 0;
    }
    public string ParseText(Conversation currentConv)
    {
        string tempLine = currentConv[currentPhrase].Texte;
        Dictionary<object, object> stats = new Dictionary<object, object>();
        stats.Add("Instructeur", 45);
        stats.Add("Pilote", 98);
        stats.Add("Assistant de bord", 98);
        stats.Add("Cuistot", 98);
        stats.Add("Mabite ", 98);

        int startArray = tempLine.IndexOf("[array:");
        int endStartArray = -1;
        if (startArray != -1)
            endStartArray = tempLine.IndexOf("]", startArray) + 1;
        int endArray = tempLine.IndexOf("[endarray]");

        while ((startArray != -1 && endStartArray != -1 && endArray != -1))
        {
            string str = tempLine.Substring(startArray + 7, endStartArray - startArray - 8);
            if (Regex.Match(str, "AvisEquipage[0,9]*").Success)
            {
                int numberOfLines = int.Parse(tempLine.Substring(startArray + 19, endStartArray - startArray - 20));
                string rep = tempLine.Substring(endStartArray, endArray - endStartArray);
                tempLine = tempLine.Remove(startArray, endArray - startArray + 10);
                
                if (!currentConv[currentPhrase].temp)
                {
                    int i = 0, j = 0;
                    foreach (KeyValuePair<object, object> obj in stats)
                    {
                        if (i >= numberOfLines && j * numberOfLines <= stats.Count)
                        {

                            j++;
                            string leReste = "";
                            Debug.Log("Nombre d'objets:"+(stats.Count-(j*multipleDisplay)));
                            if ((stats.Count - (j * numberOfLines)) >= numberOfLines )
                            {
                                Debug.Log("Adding at p=" + (currentPhrase + j) + ";with j=" + j + ", size=" + numberOfLines + ",n=" + numberOfLines);
                                leReste = "[array:AvisEquipage" + numberOfLines + "]" + rep + "[endarray]";
                            }
                            else if((stats.Count - (j * numberOfLines)) > 0)
                            {
                                Debug.Log("Adding at p=" + (currentPhrase + j) + ";with j=" + j + ", size=" + numberOfLines + ",n=" + (stats.Count - (j * numberOfLines)));
                                leReste = "[array:AvisEquipage" + (stats.Count - (j * numberOfLines)) + "]" + rep + "[endarray]";
                            }
                            if (currentConv.Count > currentPhrase + j)
                            {
                                if (currentConv[currentPhrase + j].Texte != leReste)
                                    currentConv.Add(new Phrase(currentConv[currentPhrase].Emotion, leReste, true,true));
                            }
                            else
                            {
                                currentConv.Add(new Phrase(currentConv[currentPhrase].Emotion, leReste, true,true));
                            }
                            multipleDisplay = 1;
                        }
                        if (i < numberOfLines)
                        {
                            //Debug.Log("Basic is at p=0; size=" + numberOfLines + ",n=" + (stats.Count - (0 * numberOfLines)));
                            string form = string.Format(rep, obj.Key, obj.Value);
                            tempLine = tempLine.Insert(startArray, form);
                            i++;
                        }
                    }
                    Debug.Log("------------------");
                }
                else
                {
                    int i = 0;
                    foreach (KeyValuePair<object, object> obj in stats)
                    {
                        if (i <= (numberOfLines * multipleDisplay))
                        {
                            if(i > (numberOfLines * (multipleDisplay - 1)))
                            {
                                string form = string.Format(rep, obj.Key, obj.Value);
                                tempLine = tempLine.Insert(startArray, form);
                            }
                            i++;
                        }
                        else
                        {
                            multipleDisplay++;
                            break;
                        }
                    }

                }
            }
            if (endArray >= tempLine.Length)
                endArray = tempLine.Length - 1;
            
            startArray = tempLine.IndexOf("[array:", endArray);
            if (startArray != -1)
                endStartArray = tempLine.IndexOf("]", startArray) + 1;

            endArray = tempLine.IndexOf("[endarray]", endArray);
        }
        return tempLine;
    }
}