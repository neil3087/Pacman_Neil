using System;

namespace ConsoleApp1
{
    class Program
    {
        Int32 minX = 0; // coordinate ranges
        Int32 minY = 0;
        Int32 maxX = 4;
        Int32 maxY = 4;
        String[] allowableMoves = new String[] { "PLACE", "MOVE", "LEFT", "RIGHT", "REPORT" };
        String[] allowableDirections = new String[] { "NORTH", "EAST", "SOUTH", "WEST" };
        String move;
        Int32 x;
        Int32 y;
        String face;
        String output = "";


        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Pacman!");
            ConsoleApp1.Program var = new Program();
            String command = "******"; // initialise variable to allow entry into While Loop
            String outPutStr = "";
            bool firstPass = true;
            Int32 moveStrLen;
            while (command.Trim() != "")
            {
                Console.WriteLine("Enter Command (no input will exit)");
                command = Console.ReadLine();
                moveStrLen = command.Length < 5 ? command.Length : 5; // determine length of input string to a max of 5 characters
                if (firstPass && command.Trim().Substring(0, moveStrLen).ToUpper() != "PLACE")
                {
                    Console.WriteLine("First Command Must Be A 'PLACE'!");
                }
                else
                {
                    if (command.Trim() != "")
                    {
                        outPutStr = var.Command(command);
                        if (outPutStr.Trim() != "")
                            if (firstPass && outPutStr.Trim() == "Invalid")
                            {
                                Console.WriteLine("Initial 'PLACE' command is invalid, please redo");
                            }
                            else
                            {
                                if (outPutStr.Trim() != "Invalid") // only need to display for the first PLACE ie display for REPORT
                                {
                                    Console.WriteLine(outPutStr);
                                }
                            }
                        else
                            firstPass = false; // first pass completed
                    }
                }
            }
        }


        public string Command(String instruction)
        {

            bool ifInt;
            Int32 result;
            try
            {
                String[] str = instruction.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries); // split input string into an array using comma and space as delimiters
                if (str[0] != null)
                    move = str[0].ToUpper();
                else
                    move = "";

                if (str.Length > 1) // if the input string contains more than a single element
                {
                    ifInt = Int32.TryParse(str[1], out result); // test if int
                    if (ifInt)
                        x = Int32.Parse(str[1]);
                    else
                        x = -1; // value must not be numeric, flag outside matrix/grid to disregard instruction
                    ifInt = Int32.TryParse(str[2], out result); // test if int
                    if (ifInt)
                        y = Int32.Parse(str[2]);
                    else
                        y = -1; // value must not be numeric, flag outside matrix/ grid to disregard instruction
                    if (str[3] != null)
                        face = str[3].ToUpper();
                    else
                        face = "";
                }
                bool valid = ChkInstruction(); // check command

                if (move == "REPORT")
                    output = "Output: " + x.ToString() + "," + y.ToString() + "," + face;
                else
                {
                    if (!valid)
                        output = "Invalid"; // this value will only be utilized for the initial PLACE
                    else
                        output = "";
                }
            }
            catch
            {
                output = "Error!";
            }
            return output;
        }

        private bool ChkInstruction()
        {
            bool valid = true;

            if (Array.IndexOf(allowableMoves, move) < 0) // validate move value
                valid = false;
            else
            {
                if (move == "PLACE")
                {
                    if (x < minX || x > maxX || y < minY || y > maxY || Array.IndexOf(allowableDirections, face) < 0) // validate place parameters
                    {
                        valid = false;
                    }
                }
                else
                {
                    if (move == "MOVE")
                    {
                        RevisedCoordinates rc = new RevisedCoordinates();
                        rc = GetRevisedCoorindates(x, y, face); // determine new coordinates
                        x = rc.x;
                        y = rc.y;
                    }
                    else
                    {
                        if (move == "LEFT" || move == "RIGHT")
                            GetRevisedDirection(move); // determine new direction
                    }
                }
            }

            return valid;

        }

        private void GetRevisedDirection(string leftOrRight)
        {
            if (leftOrRight == "LEFT")
            {
                switch (face)
                {
                    case "NORTH":
                        face = "WEST";
                        break;
                    case "EAST":
                        face = "NORTH";
                        break;
                    case "SOUTH":
                        face = "EAST";
                        break;
                    case "WEST":
                        face = "SOUTH";
                        break;
                }
            }
            else
            {
                switch (face)
                {
                    case "NORTH":
                        face = "EAST";
                        break;
                    case "EAST":
                        face = "SOUTH";
                        break;
                    case "SOUTH":
                        face = "WEST";
                        break;
                    case "WEST":
                        face = "NORTH";
                        break;
                }
            }
        }

        private RevisedCoordinates GetRevisedCoorindates(Int32 x, Int32 y, string face)
        {
            RevisedCoordinates rc = new RevisedCoordinates();
            rc.x = x;
            rc.y = y;
            switch (face)
            {
                case "NORTH":
                    rc.y++;
                    break;
                case "EAST":
                    rc.x++;
                    break;
                case "SOUTH":
                    rc.y--;
                    break;
                case "WEST":
                    rc.x--;
                    break;
            }
            if (rc.x < minX || rc.x > maxX || rc.y < minY || rc.y > maxY) // attempting to move outside the grid, reset to the prior coordinates
            {
                rc.x = x;
                rc.y = y;
            }
            return rc;
        }

        public class RevisedCoordinates
        {
            public Int32 x { get; set; }
            public Int32 y { get; set; }
        }
    }
}
