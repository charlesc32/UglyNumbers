using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Program
{
    static void Main(string[] args)
    {
        var input = new Dictionary<int, string>();
        int count = 0;
        using (StreamReader reader = File.OpenText(args[0]))
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            if (null == line)
                continue;
            input.Add(count, line);
            count++;
        }

        var output = new ConcurrentDictionary<int, string>();
        Parallel.ForEach(input, i=> 
        {
            output.TryAdd(i.Key, FindUglyNumbers(i.Value));   
        });

        foreach (var item in output.OrderBy(i => i.Key))
        {
            Console.WriteLine(item.Value);
        }
    }

    private static string FindUglyNumbers(string input)
    {
        var strippedInput = (input.Length > 1) ? input.TrimStart('0') : input;
        if (input[0] == '0' && input.Length > 1) strippedInput = "0" + strippedInput;

        var numOfZerosStripped = input.Length - strippedInput.Length;
        var missingPossibilities = Math.Pow(3, numOfZerosStripped);

        var basePossibilities = CreateBasePossibilities(strippedInput);
        var allPossibleCombinations = CreatePossibilities(basePossibilities);
        
        int numOfUglies = allPossibleCombinations.AsParallel().Sum(possibility => EvaluatesToUgly(possibility));
                
        return (numOfUglies * missingPossibilities).ToString();
    }

    private static int EvaluatesToUgly(string possibleUglyNum)
    {
        double result = 0;
        var currentNum = new StringBuilder();

        for (int i = 0; i < possibleUglyNum.Length; i++)
        {
            if (possibleUglyNum[i] == '+' || possibleUglyNum[i] == '-')
            {
                result += Convert.ToDouble(currentNum.ToString());
                currentNum = new StringBuilder();
            }
            currentNum.Append(possibleUglyNum[i]);
        }
        result += Convert.ToDouble(currentNum.ToString());

        if (Convert.ToDouble(result) == 0) return 1;
        if (Convert.ToDouble(result) % 2 == 0) return 1;
        if (Convert.ToDouble(result) % 3 == 0) return 1;
        if (Convert.ToDouble(result) % 5 == 0) return 1;
        if (Convert.ToDouble(result) % 7 == 0) return 1;
        
        return 0;
    }

    private static List<string> CreatePossibilities(List<List<string>> basePossibilities)
    {
        List<string> temp = basePossibilities[0];
        
        for (int i = 0; i < basePossibilities.Count - 1; i++)
        {
            temp = GetCartesianProduct(temp, basePossibilities[i + 1]);
        }

        return temp;
    }

    private static List<string> GetCartesianProduct(List<string> list1, List<string> list2)
    {
        var ret = new List<string>();
                
        var results = from possible1 in list1
                        from possible2 in list2
                        select new List<string>() { possible1 + possible2 };

        foreach (var result in results)
        {
            ret.Add(result[0]);
        }
                
        return ret;
    }

    private static List<List<string>> CreateBasePossibilities(string input)
    {
        var basePossiblities = new List<List<string>>();

        //The first character doesn't get to have a + or - in front of it so we'll just add a list of 1 to it.
        basePossiblities.Add(new List<string>() {input[0].ToString()});

        for (int i = 1; i < input.Length; i++)
        {
            basePossiblities.Add(CreateNewPositionList(input[i].ToString()));
        }

        return basePossiblities;
    }

    private static List<string> CreateNewPositionList(string input)
    {
        return new List<string>() { input, "+" + input, "-" + input };
    }
}