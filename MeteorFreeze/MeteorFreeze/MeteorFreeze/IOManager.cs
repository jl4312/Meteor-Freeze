using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace MeteorFreeze
{

    /// <summary>
    /// Class able to read and write highscore values
    /// </summary>
    public class IOManager
    {
        private static BinaryReader reader = null;
        private static BinaryWriter writer = null;

        /// <summary>
        /// Saves the given score as the new highscore in a file
        /// </summary>
        /// <param name="score"></param>
        public void SaveHighScore(int score)
        {
            try
            {
                Stream output = File.OpenWrite("highscore.hichris");
                writer = new BinaryWriter(output);
                writer.Write(score);
            }
            catch (Exception)
            {
                Console.WriteLine("Error occurred in SaveHighScore(" + score + ")!");
            }
            finally
            {
                writer.Close();
            }
        }

        /// <summary>
        /// Returns the value of the current highscore saved in the file
        /// </summary>
        /// <returns></returns>
        public int LoadHighScore()
        {
            int num = 0;
            try
            {
                Stream input = File.OpenRead("highscore.hichris");
                reader = new BinaryReader(input);
                num = reader.ReadInt32();
            }
            catch (Exception)
            {
                Console.WriteLine("Error occurred in LoadHighScore()!");
                num = -1;
            }
            finally
            {
                reader.Close();
            }
            return num;
        }

        /// <summary>
        /// Returns true if the parameter is greater than the score in the file
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public bool CompareScore(int score)
        {
            int num = 0;
            try
            {
                Stream input = File.OpenRead("highscore.hichris");
                reader = new BinaryReader(input);
                num = reader.ReadInt32();
            }
            catch (Exception)
            {
                Console.WriteLine("Error occurred in CompareScore(" + score + ")!");
            }
            finally
            {
                reader.Close();
            }
            return (num < score);
        }

        /// <summary>
        /// Returns the level from the text file as a 2-Dimensional
        /// char array
        /// </summary>
        /// <returns></returns>
        public char[,] GetLevel()
        {
            char[,] level;
            List<string> lines = new List<string>();
            StreamReader input = new StreamReader("../../GeneratedLevel.txt");
            int count = 0;
            int lineLength = 0;

            try
            {
                //Reads in each line from the file as a seperate string which is added to 
                //the list "lines", increments the count to know how many lines there are
                string line;
                while ((line = input.ReadLine()) != null)
                {
                    string data = line;
                    lineLength = data.Length;
                    //Console.WriteLine(data);
                    lines.Add(data);
                    count++;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error occured in GetLevel");
            }
            finally
            {
                input.Close();
            }

            //Sets up level as a new 2-Dimensional char array
            level = new char[lineLength, count];

            for (int i = 0; i < lineLength; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    //Sets the level at (i,j) to be the the ith character in the jth line 
                    level[i, j] = lines[j][i];
                }
            }

            return level;
        }
    }
}
