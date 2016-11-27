﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JedenTrzyPiecSiedem
{
    public class GroupedItems<T>
        where T: class
    {
        public int Count
        {
            get
            {
                return Lines.Count;
            }
        }
        public List<T> Lines { get; set; } = new List<T>();
    }

    public interface IDataListProvider<T>
        where T: class
    {
        List<T> Data { get; }
    }

    public class Engine : EngineTemplate<InfoLine, InfoLines>
    {
        public Engine(InfoLines ils) : base(ils)
        {
        }
    }

    public class EngineTemplate<T, TProvider>
        where T: class, IInfoLine
        where TProvider: IDataListProvider<T>
    {
        private TProvider infoLines;
        public EngineTemplate(TProvider ils)
        {
            infoLines = ils;
        }
        public List<GroupedItems<T>> Possibilities {
            get
            {
                return CalcPossibilities(infoLines.Data);
            }
        }

        private static List<GroupedItems<T>> CalcPossibilities(List<T> ils)
        {
            List<GroupedItems<T>> possibilities = new List<GroupedItems<T>>();
            foreach (var groupedByRow in ils.GroupBy(il => il.Row))
            {
                int row = groupedByRow.Key;
                IOrderedEnumerable<T> ilsInRow = groupedByRow.OrderBy(il => il.Col);
                if (ilsInRow.Count() > 1)
                {
                    List<T> linesToNextAdd = new List<T> { ilsInRow.First() };
                    for (int i = 1; i < ilsInRow.Count(); i++)
                    {
                        if (ilsInRow.ElementAt(i).Col - ilsInRow.ElementAt(i - 1).Col != 1)
                        {
                            AddPossibility(possibilities, linesToNextAdd);
                            linesToNextAdd = new List<T>();
                        }

                        linesToNextAdd.Add(ilsInRow.ElementAt(i));
                    }
                    AddPossibility(possibilities, linesToNextAdd);
                } else if (ilsInRow.Any())
                {
                    AddPossibility(possibilities, new List<T> { ilsInRow.First() });
                }
            }
            return possibilities;
        }
        private static void AddPossibility(List<GroupedItems<T>> possibilities, List<T> ils)
        {
            possibilities.Add(new GroupedItems<T>
            {
                Lines = ils
            });
        }

        public static bool CheckPairs(List<int> toCheck)
        {
            var groups = toCheck.GroupBy(i => i);
            return groups.ToList().TrueForAll(g => g.Count() % 2 == 0);
        }

        // some has to be different
        public static bool CheckSomeWithPairs(List<int> toCheck, List<int> some)
        {
            var toCheckCopy = new List<int>(toCheck);
            var groups = toCheckCopy.GroupBy(i => i);

            some = some.Distinct().ToList();
            Dictionary<int, IGrouping<int, int>> somes = new Dictionary<int, IGrouping<int, int>>();

            foreach (int s in some)
            {
                somes.Add(s, groups.FirstOrDefault(g => g.Key == s));
            }

            if (somes.All(s => s.Value != null))
            {
                int firstCount = somes.First().Value.Count();
                if (somes.All(s => s.Value.Count() == firstCount))
                {
                    toCheckCopy.RemoveAll(i => some.Any(s => s == i));
                    return CheckPairs(toCheckCopy);
                }
            }
            return false;
        }

        public static bool Check145WithPairs(List<int> toCheck)
        {
            return CheckSomeWithPairs(toCheck, new List<int> { 1, 4, 5 });
        }

        public static bool Check123WithPairs(List<int> toCheck)
        {
            return CheckSomeWithPairs(toCheck, new List<int> { 1, 2, 3 });
        }

        public List<T> FindBestMove()
        {
            if (Possibilities.Count == 0)
            {
                return new List<T>();
            }
            else
            {
                var tested = Test(Possibilities);
                if (tested.Any())
                    return tested;
                else
                {
                    //int max = Possibilities.Max(g => g.Count);
                    //GroupedItems<T> gi = Possibilities.First(g => g.Count == max);
                    //return new List<T> { gi.Lines.First() };
                    var lessLost = FindLessLostMoveByOnes(infoLines.Data);
                    if (lessLost.Any())
                    {
                        return lessLost;
                    }

                    int min = Possibilities.Min(g => g.Count);
                    GroupedItems<T> gi = Possibilities.First(g => g.Count == min);
                    return new List<T> { gi.Lines.First() };



                    // TODO: find such move, after which next move won't be able to win. If it's not possible, make above move or random
                }
            }
        }

        // TODO: make less lost move for groups
        private static List<T> FindLessLostMoveByOnes(List<GroupedItems<T>> toCheck)
        {

            //Random r = new Random();
            //foreach (int i in Enumerable.Range(0, toCheck.Count).OrderBy(x => r.Next()))
            //{
            //    T il = toCheck.ElementAt(i);
            //    //}
            //    //foreach (T il in toCheck)
            //    //{
            //    List<T> copy = new List<T>(toCheck);
            //    copy.Remove(il);
            //    if (!Test(CalcPossibilities(copy)).Any())
            //    {
            //        return new List<T> { il };
            //    }
            //}
            return new List<T>();
        }

        // TODO: make less lost move for groups
        private static List<T> FindLessLostMoveByOnes(List<T> toCheck)
        {
            Random r = new Random();
            foreach (int i in Enumerable.Range(0, toCheck.Count).OrderBy(x => r.Next()))
            {
                T il = toCheck.ElementAt(i);

                List<T> copy = new List<T>(toCheck);
                copy.Remove(il);
                if (!Test(CalcPossibilities(copy)).Any())
                {
                    return new List<T> { il };
                }
            }
            return new List<T>();
        }


        class OKException : Exception
        {
            public int before;
            public int index;
            public int count;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="toCheck"></param>
        /// <returns>list to remove</returns>
        private static List<T> Test(List<GroupedItems<T>> toCheck)
        {
            var groups = toCheck.GroupBy(i => i.Count);
            var toCheckInts = toCheck.Select(i => i.Count);

            if (toCheck.Count(gi => gi.Count > 1) == 1)
            {
                GroupedItems<T> g = toCheck.First(gi => gi.Count > 1);


                var toCheckList = toCheckInts.ToList();
                toCheckList.Remove(g.Count);
                if (!CheckPairs(toCheckList))
                {
                    return GetRemoved(g, 0, g.Count);
                } else
                {
                    toCheckList.Add(1);
                    if (!CheckPairs(toCheckList))
                    {
                        return GetRemoved(g, 0, g.Count - 1);
                    }
                }
            }

            try
            {
                Random r = new Random();
                foreach (int i in Enumerable.Range(0, groups.Count()).OrderBy(x => r.Next()))
                {
                    IGrouping<int, GroupedItems<T>> group = groups.ElementAt(i);
                    IterateOverBefore(group.Key, (afterRemove, index, count) =>
                    {
                        var toCheckList = toCheckInts.ToList();
                        toCheckList.Remove(group.Key);
                        toCheckList.AddRange(afterRemove);
                        if (CheckPairs(toCheckList) || Check123WithPairs(toCheckList) || Check145WithPairs(toCheckList))
                        {
                            throw new OKException
                            {
                                before = group.Key,
                                index = index,
                                count = count
                            };
                        }
                    });
                }
            } catch (OKException e)
            {
                var groupedToRemove = toCheck.Where(l => l.Count == e.before);
                if (!groupedToRemove.Any())
                {
                    throw new Exception("blad");
                } else
                {
                    Random r = new Random();
                    int index = Enumerable.Range(0, groupedToRemove.Count()).OrderBy(x => r.Next()).First();
                    return GetRemoved(groupedToRemove.ElementAt(index), e.index, e.count);
                }
            }
            return new List<T>();
        }

        private static void IterateOverBefore(int before, Action<List<int>, int, int> iteration)
        {
            Random r = new Random();
            foreach (int index in Enumerable.Range(0, before).OrderBy(x => r.Next()))
            {
                foreach (int count in Enumerable.Range(1, before - index).OrderBy(x => r.Next()))
                {
                    iteration(RemoveFrom(before, index, count), index, count);
                }
            }
        }
        
        public static List<int> RemoveFrom(int before, int index, int count)
        {
            if (count == before)
            {
                return new List<int>();
            } else if (index == 0 || index + count == before)
            {
                return new List<int> { before - count };
            } else
            {
                return new List<int> { index, before - index - count };
            }
        }

        /// <summary>
        /// Assumption: grouped items have the same row and their ordered difference is 1
        /// </summary>
        /// <param name="grouped"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns>items to be removed</returns>
        public static List<T> GetRemoved(GroupedItems<T> grouped, int index, int count)
        {
            if (grouped.Count == 1)
            {
                return new List<T>(grouped.Lines);
            }
            else
            {
                if ((grouped.Count < index + count) || count <= 0 || index < 0)
                    throw new Exception("error");

                if (count == grouped.Count)
                {
                    return new List<T>(grouped.Lines);
                }
                else
                {
                    return grouped.Lines.GetRange(index, count);
                }
            }
        }
    }
}
