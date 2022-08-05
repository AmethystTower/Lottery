using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace Kunz_Lottery
{
    class Program
    {
        //import & initialize _getch() function.
        [DllImport("msvcrt.dll")]
        static extern char _getch();

        static void Main()
        {
            Console.WriteLine("\n\tWelcome. Do you want to play on normal or on EXTREME?.\n\n\tN - Normal: Play a normal round.\n\tE - EXTREME: Play and calculate statistics from a million rounds.");
            bool EXTREME;

            for (; ; )
            {
                char text = _getch();
                text = char.ToLower(text);

                if (text == 'n')
                {
                    EXTREME = false;
                    Console.Clear();
                    break;
                }
                else if (text == 'e')
                {
                    EXTREME = true;
                    Console.Clear();
                    break;
                }
            }

            //have to initialize this stuff anyway because otherwise the compiler will complain about not initialized variables.
            //if(EXTREME)
            long[] total = new long[7];

            for (byte i = 0; i < 7; i++)
                total[i] = 0;

            //this is used for the EXTREME game mode.
            long totalRounds = 1000000;

            List<int> selectedNumbers = new List<int>();
            List<int> randomNumbers = new List<int>();
            bool[] results = new bool[6];
            int countResults = 0;

            //the actual program.
            for (; ; )
            {
                Console.WriteLine("\n\tWelcome to some lottery game.\n\tObjective: Choose 6 numbers between 1-49 and try to guess all generated \tnumbers correctly.\n\n\tPress any key to continue.");

                Console.ReadKey();

                Console.Clear();
                selectedNumbers = NumberSelection();

                //if EXTREME mode is enabled, calculate over a million rounds and then display statistics.
                if (EXTREME)
                {
                    Console.WriteLine("\n\tCalculating " + totalRounds + " rounds, please be patient...");
                    Thread.Sleep(3000);

                    for (long extremeCount = 0; extremeCount < totalRounds; extremeCount++)
                    {
                        //generate computer numbers.
                        randomNumbers = GenerateNumbers(false);

                        //this function simply checks for the amount of correct numbers and returns it to the integer directly.
                        countResults = CompareNumbersExtreme(selectedNumbers, randomNumbers);

                        switch (countResults)
                        {
                            case 0:
                                total[0]++;
                                break;
                            case 1:
                                total[1]++;
                                break;
                            case 2:
                                total[2]++;
                                break;
                            case 3:
                                total[3]++;
                                break;
                            case 4:
                                total[4]++;
                                break;
                            case 5:
                                total[5]++;
                                break;
                            case 6:
                                total[6]++;
                                break;
                        }

                        //clear list array from previous generated numbers that are no longer needed.
                        randomNumbers.Clear();
                    }

                    Console.Clear();
                    Console.WriteLine("\n\n\tResults - x amount out of 6 numbers were correct this many times out of \t" + totalRounds + " rounds: \n\n\t0 Numbers - " + total[0] + "\n\t1 Number - " + total[1] + "\n\t2 Numbers - " + total[2] + "\n\t3 Numbers - " + total[3] + "\n\t4 Numbers - " + total[4] + "\n\t5 Numbers - " + total[5] + "\n\t6 Numbers - " + total[6]);
                    Console.WriteLine("\n\n\tPercentage Results: \n\t0 Numbers - " + (((float)total[0] / (float)totalRounds) * 100).ToString("0.000") + "%\n\t1 Number - " + (((float)total[1] / (float)totalRounds) * 100).ToString("0.000") + "%\n\t2 Numbers - " + (((float)total[2] / (float)totalRounds) * 100).ToString("0.000") + "%\n\t3 Numbers - " + (((float)total[3] / (float)totalRounds) * 100).ToString("0.000") + "%\n\t4 Numbers - " + (((float)total[4] / (float)totalRounds) * 100).ToString("0.000") + "%\n\t5 Numbers - " + (((float)total[5] / (float)totalRounds) * 100).ToString("0.000") + "%\n\t6 Numbers - " + (((float)total[6] / (float)totalRounds) * 100).ToString("0.000") + "%");


                    Console.WriteLine("\n\n\tPress ENTER to restart or any other key to close the application.");

                    if (Console.ReadKey().Key != ConsoleKey.Enter)
                        break;
                    else
                        Console.Clear();
                }
                else
                {
                    randomNumbers = GenerateNumbers(true);
                    results = CompareNumbers(selectedNumbers, randomNumbers);
                }

                Console.WriteLine("\n\tThe computer has generated its numbers and you have selected yours. How \tmany did you guess correctly?\n\n\tPress any key to continue.");
                Console.ReadKey();
                Console.Clear();

                //show RESULTS. Use Write() so that everything can be printed in one line.
                Console.Write("\nYour Numbers:\t\t");

                foreach (byte num in selectedNumbers)
                    Console.Write("\t" + num);

                Console.Write("\n\t\t\t");

                for (byte i = 0; i < 6; i++)
                {
                    Thread.Sleep(1000);

                    if (results[i])
                    {
                        Console.Write("\t" + "+");
                        countResults++;
                    }
                    else
                        Console.Write("\t" + "-");
                }

                Thread.Sleep(2000);
                Console.Write("\n\n\nGenerated Numbers:\t");

                foreach (byte num in randomNumbers)
                {
                    Console.Write("\t" + num);
                    Thread.Sleep(1000);
                }

                Console.WriteLine("\n\n\tYou've guessed " + countResults + " numbers correctly out of " + results.Length + " generated numbers.\n\n\tPress ENTER to restart or any other key to close the application.");

                if (Console.ReadKey().Key != ConsoleKey.Enter)
                    break;
                else
                    Console.Clear();
            }
        }

        //let the user choose 6 numbers between 1-49. no invalid inputs allowed.
        static List<int> NumberSelection()
        {
            List<int> selectedNumbers = new List<int>();

            for (byte i = 0; i < 6; i++)
            {
                Console.Write("\n\tPlease input number " + (i + 1) + ":\n\tNOTE: The number MUST be between 1-49 and has to be unique. ");

                if (selectedNumbers.Count > 0)
                {
                    Console.WriteLine("\n\n\tCurrently Selected numbers: ");
                    foreach (byte num in selectedNumbers)
                        Console.Write("\t" + num + "\n");
                }

                string input = Console.ReadLine();

                //check if input is valid and save resulting integer.
                if (!int.TryParse(input, out int number) || number < 1 || number > 49 || selectedNumbers.Contains(number))
                {
                    Console.WriteLine("\n\tInvalid input! Try again...");

                    //reduce number by -1.
                    i--;

                    Thread.Sleep(2000);
                }
                //add integer to list array.
                else
                    selectedNumbers.Add(number);

                Console.Clear();
            }

            Console.Clear();
            return selectedNumbers;
        }

        //the computer generates 6 numbers here. skip wait and clear functions when in extreme mode, since it needs to calculate more than one round. Console.Clear() slows down the process massively.
        static List<int> GenerateNumbers(bool wait)
        {
            if (wait)
            {
                Console.WriteLine("\n\tThe computer is generating its numbers between 1-49...");
                Thread.Sleep(3000);
            }

            //generate list for this function.
            List<int> randomNumbers = new List<int>();
            Random computer = new Random();

            for (byte i = 0; i < 6; i++)
            {
                int num = computer.Next(1, 49 + 1);   //+1 for the second number because otherwise it will equal 48 (since it counts the 0).

                //check if number exists already, if yes then re-roll number. we don't want duplicates here...
                if (!randomNumbers.Contains(num))
                    randomNumbers.Add(num);
                else
                    i--;
            }

            if (wait)
                Console.Clear();

            return randomNumbers;
        }

        //return boolean array with information for each number being correct or wrong, in order to show the user which of his numbers were correct or wrong.
        static bool[] CompareNumbers(List<int> selectedNumbers, List<int> randomNumbers)
        {
            bool[] results = new bool[6];
            short pos = 0;

            foreach (byte num in selectedNumbers)
            {
                for (byte i = 0; i < 6; i++)
                {
                    //set it to false by default and set it to true if one of the numbers matches.
                    results[pos] = false;

                    //number is correct? set this instance to true and skip rest of the rounds.
                    if (randomNumbers[i] == num)
                    {
                        results[pos] = true;
                        break;
                    }
                }

                pos++;
            }

            return results;
        }

        //compare numbers for the extreme mode and directly return the amount of correct numbers, instead of a boolean array. we don't need to know which specific numbers are correct here.
        static byte CompareNumbersExtreme(List<int> selectedNumbers, List<int> randomNumbers)
        {
            byte hits = 0;
            foreach (byte num in selectedNumbers)
            {
                for (byte i = 0; i < 6; i++)
                {
                    if (randomNumbers[i] == num)
                    {
                        hits++;
                        break;
                    }
                }
            }

            return hits;
        }

        /* //very pointless to have this when there can be just another compare numbers function that returns the amount of correct numbers anyway.
        static int EXTREMECalc(List<int> selectedNumbers, bool[] results, int countResults)
        {
            for (int i = 0; i < 6; i++)
            {
                if (results[i])
                    countResults++;
            }

            return countResults;
        }
        */
    }
}
