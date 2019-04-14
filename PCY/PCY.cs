using System;
using System.Collections.Generic;
using System.Linq;

namespace PCY
{
    class PCY
    {
        #region Helping stuff

        public class MyEqualityComparer : IEqualityComparer<int[]>
        {
            public bool Equals(int[] x, int[] y)
            {
                if (x.Length != y.Length)
                {
                    return false;
                }
                for (int i = 0; i < x.Length; i++)
                {
                    if (x[i] != y[i])
                    {
                        return false;
                    }
                }
                return true;
            }

            public int GetHashCode(int[] obj)
            {
                int result = 17;
                for (int i = 0; i < obj.Length; i++)
                {
                    unchecked
                    {
                        result = result * 23 + obj[i];
                    }
                }
                return result;
            }
        }

        public class Basket
        {
            public List<int> Items;
        }

        #endregion

        static void Main(string[] args)
        {

            var listOfBaskets = new List<Basket>();

            #region Testing region

            //int numberOfBuckets;
            //int supportThreshold;
            //int numberOfBaskets;
            //int[] itemsCounter;

            //const Int32 BufferSize = 128;
            //using (var fileStream = File.OpenRead("S:\\Projekti\\PCY\\PCY\\bin\\Debug\\R2.in"))
            //using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            //{
            //    //učitaj broj košara
            //    numberOfBaskets = Convert.ToInt32(streamReader.ReadLine());

            //    //s ima vrijednost 0-1
            //    var s = Convert.ToDouble(streamReader.ReadLine());

            //    //učitaj broj pretinaca
            //    numberOfBuckets = Convert.ToInt32(streamReader.ReadLine());

            //    //cjelobrojna vrijednost (floor)
            //    supportThreshold = Convert.ToInt32(s * numberOfBaskets);

            //    //brojač predmeta
            //    itemsCounter = new int[100 + 1];

            //    #region First pass


            //    for (var i = 0; i < numberOfBaskets; i++)
            //    {
            //        var basket = new Basket();
            //        var listOfStrings = streamReader.ReadLine().Split().ToList();
            //        basket.Items = listOfStrings.OrderBy(x => x).Select(int.Parse).ToList();
            //        listOfBaskets.Add(basket);

            //        foreach (var item in basket.Items)
            //        {
            //            itemsCounter[item]++;
            //        }
            //    }
            //    #endregion

            //}

            #endregion

            #region Reading and initializing data

            ////učitaj broj košara
            var numberOfBaskets = Convert.ToInt32(Console.ReadLine());

            //s ima vrijednost 0-1
            var s = Convert.ToDouble(Console.ReadLine());

            //učitaj broj pretinaca
            var numberOfBuckets = Convert.ToInt32(Console.ReadLine());

            //cjelobrojna vrijednost (floor)
            var supportThreshold = Convert.ToInt32(s * numberOfBaskets);

            //brojač predmeta
            var itemsCounter = new int[100 + 1];

            #endregion

            #region First pass


            for (var i = 0; i < numberOfBaskets; i++)
            {
                var basket = new Basket();
                var listOfStrings = Console.ReadLine().Split().ToList();
                basket.Items = listOfStrings.OrderBy(x => x).Select(int.Parse).ToList();
                listOfBaskets.Add(basket);

                foreach (var item in basket.Items)
                {
                    itemsCounter[item]++;
                }
            }

            #endregion

            #region Second pass

            var buckets = new int[numberOfBuckets];
            for (var index = 0; index < numberOfBaskets; index++)
            {
                //potrebna provjera ako ima manje od 2 itema??
                //sažmi par predmeta u pretinac
                //za svaki par predmeta {i,j}
                var basket = listOfBaskets[index];
                for (var i = 0; i < basket.Items.Count - 1; i++)
                {
                    for (var j = i + 1; j < basket.Items.Count; j++)
                    {
                        var item1 = basket.Items[i];
                        var item2 = basket.Items[j];

                        //oba predmeta moraju biti česta
                        if (itemsCounter[item1] >= supportThreshold && itemsCounter[item2] >= supportThreshold)
                        {
                            var k = (item1 * itemsCounter[item1] + item2) % numberOfBuckets;
                            buckets[k]++;
                        }
                    }
                }
            }

            #endregion

            #region Third pass

            //brojanje parova
            //mapa - ključ par predmeta [i,j], vrijednost broj ponavljanja

            var dictionaryPairSum = new Dictionary<int[], int>(new MyEqualityComparer());
            for (var index = 0; index < numberOfBaskets; index++)
            {
                //potrebna provjera ako ima manje od 2 itema??
                //za svaki par predmeta {i,j}
                var basket = listOfBaskets[index];
                for (var i = 0; i < basket.Items.Count - 1; i++)
                {
                    for (var j = i + 1; j < basket.Items.Count; j++)
                    {
                        var item1 = basket.Items[i];
                        var item2 = basket.Items[j];

                        //oba predmeta moraju biti česta
                        if (itemsCounter[item1] >= supportThreshold && itemsCounter[item2] >= supportThreshold)
                        {
                            var k = (item1 * itemsCounter[item1] + item2) % numberOfBuckets;
                            if (buckets[k] >= supportThreshold)
                            {
                                var proba = new[] { item1, item2 };
                                Array.Sort(proba);
                                if (dictionaryPairSum.ContainsKey(proba))
                                {
                                    dictionaryPairSum[proba]++;

                                }
                                else
                                {
                                    dictionaryPairSum.Add(proba, 1);
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region Output

            var frequentPairs = dictionaryPairSum.Where(x => x.Value >= supportThreshold).OrderByDescending(x => x.Value).ToList();
            var m = itemsCounter.Count(x => x >= supportThreshold);
            var A = m * (m - 1) / 2;
            Console.WriteLine(A);
            var P = frequentPairs.Count;
            Console.WriteLine(P);

            foreach (var pair in frequentPairs)
            {
                Console.WriteLine(pair.Value);
            }
            #endregion

        }
    }
}
