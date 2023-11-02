using System;
using System.Collections.Generic;
using System.Linq;

namespace Sencond_attempt__different_equasions
{
    class Program
    {
        struct Body
        {
            public double posX; //m
            public double posY;
            public double xVelocity; // m/s
            public double yVelocity;
            public double mass; //kg
            public string name;
            public double radius; //m
            public string colour;
        }

        static Body addObject(double X, double Y, double xVel, double yVel, double mas, double rad, string na, string color)
        {
            Body temp;
            temp.posX = X;
            temp.posY = Y;
            temp.xVelocity = xVel;
            temp.yVelocity = yVel;
            temp.mass = mas;
            temp.name = na;
            temp.radius = rad;
            temp.colour = color;
            return temp;
        }

        static double distanceCalc(double x1, double y1, double x2, double y2)
        {
            double distance = Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
            //distance = square root of ((difference in x axis) ^ 2 + (difference in y axis ^ 2))
            return distance;
        }

        static double[] vectorClac(double x1, double y1, double x2, double y2)
        {
            //function made just in case it is needed.
            double[] vector = new double[2];
            vector[0] = x2 - x1;
            vector[1] = y2 - y1;
            return vector;
        }

        static double gravPullForceCalc(double mass1, double mass2, double distance)
        {
            double gravConstant = 6.6743e-11;

            double gravitationalPull = (mass1 * mass2) / Math.Pow(distance, 2);
            //gravitational pull = gravitational constant * (mass1*mass2) / distance^2

            gravitationalPull = gravConstant * gravitationalPull;
            //adding the gravitational constant

            return gravitationalPull;
        }

        static double largestDistanceFromCenter(List<Body> Objects)
        {
            double largestDistance = 0;

            for (int i = 0; i < Objects.Count; i++)
            {
                if (Math.Sqrt(Objects[i].posX * Objects[i].posX + Objects[i].posY * Objects[i].posY) + Objects[i].radius > largestDistance)
                {
                    largestDistance = Math.Sqrt(Objects[i].posX * Objects[i].posX + Objects[i].posY * Objects[i].posY);
                    largestDistance += Objects[i].radius;
                }
            }
            for (int i = 0; i < Objects.Count; i++)
            {
                if (largestDistance < Objects[i].radius * 2)
                {
                    largestDistance = Objects[i].radius * 2;
                }
            }
            return largestDistance;
        }

        static double[,] accelerationCalc(List<Body> Objects)
        {
            double[,] accelerations = new double[Objects.Count, 2];
            double distance;
            double gravPullForce;
            double[] unitVector = new double[2];

            for (int body1 = 0; body1 < Objects.Count; body1++)
            {
                for (int body2 = 0; body2 < Objects.Count; body2++)
                {
                    if (body1 != body2)
                    {

                        distance = distanceCalc(Objects[body1].posX, Objects[body1].posY, Objects[body2].posX, Objects[body2].posY);
                        unitVector = vectorClac(Objects[body1].posX, Objects[body1].posY, Objects[body2].posX, Objects[body2].posY);

                        unitVector[0] = unitVector[0] / distance;
                        unitVector[1] = unitVector[1] / distance;

                        gravPullForce = gravPullForceCalc(Objects[body1].mass, Objects[body2].mass, distance);

                        for (int axis = 0; axis < unitVector.Length; axis++)
                        {
                            accelerations[body1, axis] += (unitVector[axis] * gravPullForce) / Objects[body1].mass;
                        }

                    }
                }
            }
            return accelerations;
        }

        static List<Body> velocityUpdate(List<Body> Objects, double[,] accelerations, double timePerTick)
        {
            Body holder;
            for (int i = 0; i < Objects.Count; i++)
            {
                holder = Objects[i];

                holder.xVelocity = holder.xVelocity + accelerations[i, 0] * timePerTick;
                //x axis velocity += x axis acceleration
                holder.yVelocity = holder.yVelocity + accelerations[i, 1] * timePerTick;
                //y axis velocity += y axis acceleration

                Objects[i] = holder;
            }
            return Objects;
        }

        static List<Body> positionUpdate(List<Body> Objects, double timePerTick)
        {
            Body holder;
            for (int i = 0; i < Objects.Count; i++)
            {
                holder = Objects[i];

                holder.posX = holder.posX + Objects[i].xVelocity * timePerTick;
                //x axis position += x axis velocity
                holder.posY = holder.posY + Objects[i].yVelocity * timePerTick;
                //y axis position += y axis velocity

                Objects[i] = holder;
            }
            return Objects;
        }

        static void colourChanger(string colour)
        {
            if (colour == "yellow")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (colour == "dark yellow")
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }
            else if (colour == "green")
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (colour == "dark green")
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            else if (colour == "red")
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (colour == "dark red")
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }
            else if (colour == "magenta")
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
            }
            else if (colour == "dark magenta")
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            }
            else if (colour == "blue")
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else if (colour == "dark blue")
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
            }
            else if (colour == "cyan")
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            else if (colour == "dark cyan")
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
            }
            else if (colour == "white")
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        static void scroll(int arrayWidth, int row)
        {
            Console.CursorLeft = arrayWidth * 2 + 2;
            Console.CursorTop = row;
        }

        static List<string[,]> trailQueueAdd(List<string[,]> trailsQueue, int trailLenght, List<Body> Objects)
        {
            List<string[,]> trailsQueueReplacement = new List<string[,]> { };

            for (int i = 0; i < Objects.Count; i++)
            {
                trailsQueueReplacement.Add(new string[trailLenght, 4]);
                for (int j = 0; j < trailLenght; j++)
                {
                    trailsQueueReplacement[i][j, 0] = Objects[i].name;
                }

                //initialising the list to be full of empty 2d arrays that will be recycled as the trail fades
                //if a trail queue is inputted, then it will copy all the contents of the input queueu into the queue with the same lenght as the Ojects list

                //the way the list will be used is:
                //the list contains one array for each planet
                //each 2d array contains five arrays with
                //the name of the body that will be unchanging,
                //the x and y position where that piece of the trail needs to be placed, and
                //and how long ago the that piece of the trail was first placed (based on how many times it has been drawn)
                //when the trail has been drawn a certain number of times it gets removed and in the same 2d array the location is changed and timer is reset
            }

            if (trailsQueue.Count <= trailsQueueReplacement.Count)
            {
                for (int i = 0; i < trailsQueue.Count; i++)
                {
                    for (int j = 0; j < trailLenght; j++)
                    {
                        trailsQueueReplacement[i][j, 1] = trailsQueue[i][j, 1];
                        trailsQueueReplacement[i][j, 2] = trailsQueue[i][j, 2];
                    }
                }
            }

            return trailsQueueReplacement;
        }

        static void cleanTrails(ref List<string[,]> trails, int trailLenght, int planetCount)
        {
            for (int i = 0; i < planetCount; i++)
            {
                for (int j = 0; j < trailLenght; j++)
                {
                    trails[i][j, 3] = null;
                }
            }
        }

        static List<Body> planetRemover(List<Body> Objects, int id)
        {
            List<Body> newObjects = new List<Body>();
            for (int i = 0; i < Objects.Count; i++)
            {
                if (i != id)
                {
                    newObjects.Add(Objects[i]);
                }
            }
            return newObjects;
        }

        static List<Body> bodyCopy(List<Body> original)
        {
            List<Body> Copy = new List<Body>();
            for (int i = 0; i < original.Count; i++)
            {
                Copy.Add(original[i]);
            }
            return Copy;
        }

        static List<Body> addPlanet(List<Body> Objects, int arrayHeight, int arrayWidth, double largestDistance, List<string[,]> trailsQueue, int trailLenght, double moveX, double moveY)
        {
            Body newPlanet = new Body();
            List<Body> newObjects = bodyCopy(Objects);
            List<string[,]> newtrailsQueue = trailQueueAdd(trailsQueue, trailLenght, newObjects);
            //List<Body> backUpObjects = Objects;
            ConsoleKeyInfo enteredKey;

            Console.CursorVisible = true;
            arrayWidth = (Console.WindowWidth - 50) / 2 - 2;

            double cursorX = arrayWidth;
            double cursorY = arrayHeight / 2 + 1;
            double jumpOne = 0;

            double posX = 0;
            double posY = 0;
            double Xvelocity = 0;
            double Yvelocity = 0;
            double mass = 1000000;
            double radius = 200;
            string name = "????";
            string colour = "red";

            int Adjust = (arrayWidth - 1) / 2 - (arrayHeight - 1) / 2;

            bool accept = false;
            bool adjustingVelocity = false;
            bool simulating = false;
            bool doneSimulating = false;
            bool showingInstructions = true;
            bool cancel = false;

            double solarMass = 1.989e30;
            double time;
            double timeLastImage;
            int timePerTickInSec;
            int timeScale = 60 * 60;
            timePerTickInSec = 60;
            int planetCount;
            double largestDistanceBackup;
            bool collision;

            double posXScroll = largestDistance / (arrayWidth / 2) * 1.6;
            double posYScroll = largestDistance / (arrayHeight / 2);

            int actualTimeMilliseconds = int.Parse(DateTime.Now.Millisecond.ToString());

            double[,] acceleration;

            Console.CursorTop = (int)Math.Round(cursorY);
            Console.CursorLeft = (int)Math.Round(cursorX);

            Console.Clear();

            boxPrinter(arrayHeight, arrayWidth);

            printInstructions(showingInstructions, arrayWidth, true, adjustingVelocity);

            displayPlanetStats(newObjects, newObjects.Count - 1, arrayWidth);

            visualPosition(newObjects, ref largestDistance, ref trailsQueue, trailLenght, 0, 0, (int)moveX, (int)moveY, false, arrayHeight, arrayWidth);

            newObjects = bodyCopy(Objects);

            newObjects.Add(addObject(posX, posY, Xvelocity, Yvelocity, mass, radius, name, colour));

            while (!accept)
            {
                Console.CursorLeft = arrayWidth - 8;
                Console.CursorTop = 1;
                Console.Write(">ADDING  PLANET<");

                planetCount = newObjects.Count;

                time = 0;
                timeLastImage = 1;

                actualTimeMilliseconds = int.Parse(DateTime.Now.Millisecond.ToString());

                visualPosition(Objects, ref largestDistance, ref trailsQueue, trailLenght, -1, 0, (int)moveX, (int)moveY, false, arrayHeight, arrayWidth);

                Console.Write("Current Time Scale: " + secondsSimplifier(timeScale * 30) + " per second     ");
                Console.Write("\nTime per tick: " + secondsSimplifier(timePerTickInSec) + "     ");

                Console.CursorTop = (int)Math.Round(cursorY);
                Console.CursorLeft = (int)Math.Round(cursorX);
                Console.Write(name.Substring(2));
                Console.CursorTop = (int)Math.Round(cursorY);
                Console.CursorLeft = (int)Math.Round(cursorX);

                enteredKey = Console.ReadKey(true);


                if (enteredKey.Key == ConsoleKey.OemComma)
                {
                    largestDistance = largestDistance * 1.1;
                    moveX = moveX / 1.1;
                    posX = posX * 1.1;
                    moveY = moveY / 1.1;
                    posY = posY * 1.1;
                    //zoom out
                    posXScroll = largestDistance / (arrayWidth / 2) * 1.85;
                    posYScroll = largestDistance / (arrayHeight / 2);

                    boxCleaner(arrayHeight, arrayWidth);
                    visualPosition(Objects, ref largestDistance, ref trailsQueue, trailLenght, -1, 0, (int)moveX, (int)moveY, false, arrayHeight, arrayWidth);

                    Console.CursorLeft = arrayWidth - 8;
                    Console.CursorTop = 1;
                    Console.Write(">ADDING  PLANET<");
                }
                else if (enteredKey.Key == ConsoleKey.OemPeriod && largestDistance > 10)
                {
                    largestDistance = largestDistance / 1.1;
                    moveX = moveX * 1.1;
                    posX = posX / 1.1;
                    moveY = moveY * 1.1;
                    posY = posY / 1.1;
                    //zoom in
                    posXScroll = largestDistance / (arrayWidth / 2) * 1.85;
                    posYScroll = largestDistance / (arrayHeight / 2);

                    boxCleaner(arrayHeight, arrayWidth);
                    visualPosition(Objects, ref largestDistance, ref trailsQueue, trailLenght, -1, 0, (int)moveX, (int)moveY, false, arrayHeight, arrayWidth);

                    Console.CursorLeft = arrayWidth - 8;
                    Console.CursorTop = 1;
                    Console.Write(">ADDING  PLANET<");
                }
                else if (enteredKey.Key == ConsoleKey.Enter)
                {
                    simulating = true;

                    boxCleaner(arrayHeight, arrayWidth);

                    newObjects = bodyCopy(Objects);

                    newObjects.Add(addObject(posX, posY, Xvelocity, Yvelocity, mass, radius, name, colour));

                    newtrailsQueue = trailQueueAdd(trailsQueue, trailLenght, newObjects);
                }
                else if (enteredKey.Key == ConsoleKey.C)
                {
                    cancel = true;
                    accept = true;
                }
                else if (enteredKey.Key == ConsoleKey.Spacebar)
                {
                    adjustingVelocity = !adjustingVelocity;
                    printInstructions(showingInstructions, arrayWidth, true, adjustingVelocity);
                }
                else if (enteredKey.Key == ConsoleKey.UpArrow)
                {
                    if (mass < solarMass * 100)
                    {
                        mass *= 2;
                        radius = Math.Round(radius * Math.Pow(2, 1.0 / 3.0));
                        //need to raise 2 to the power of 1.0/3.0 to cube root it and not 1/3 because 1/3 return the closest integer; 0
                        //1.0/3.0 correctly returns a float, although 1/3 cannot be represent as a floating point number, it gives a floating point error
                        //so the answer needs then to be rounded
                    }
                    newObjects = bodyCopy(Objects);

                    newObjects.Add(addObject(posX, posY, Xvelocity, Yvelocity, mass, radius, name, colour));

                    clearCentredStats(arrayWidth);

                    displayPlanetStats(newObjects, newObjects.Count - 1, arrayWidth);

                    newObjects = bodyCopy(Objects);
                }
                else if (enteredKey.Key == ConsoleKey.DownArrow)
                {
                    if (mass > 1)
                    {
                        mass /= 2;
                        radius = Math.Round(radius / Math.Pow(2, 1.0 / 3.0));
                        //need to raise 2 to the power of 1.0/3.0 to cube root it and not 1/3 because 1/3 return the closest integer; 0
                        //1.0/3.0 correctly returns a float, although 1/3 cannot be represent as a floating point number, it gives a floating point error
                        //so the answer needs then to be rounded
                    }
                    newObjects = bodyCopy(Objects);

                    newObjects.Add(addObject(posX, posY, Xvelocity, Yvelocity, mass, radius, name, colour));

                    clearCentredStats(arrayWidth);

                    displayPlanetStats(newObjects, newObjects.Count - 1, arrayWidth);

                    newObjects = bodyCopy(Objects);
                }
                else if (enteredKey.Key == ConsoleKey.RightArrow)
                {
                    timeChanger(ref timeScale, ref timePerTickInSec, true);
                    //speed up
                }
                else if (enteredKey.Key == ConsoleKey.LeftArrow)
                {
                    timeChanger(ref timeScale, ref timePerTickInSec, false);
                    //slow down
                }
                else if (enteredKey.Key == ConsoleKey.P)
                {
                    accept = true;
                }
                else
                {
                    if (!adjustingVelocity)
                    {
                        if (enteredKey.KeyChar == 'A')
                        {
                            Console.Write("  ");
                            if (cursorX - 1 > 1)
                            {
                                posX -= posXScroll;
                                cursorX -= 2;
                            }
                            newObjects = bodyCopy(Objects);

                            newObjects.Add(addObject(posX, posY, Xvelocity, Yvelocity, mass, radius, name, colour));

                            clearCentredStats(arrayWidth);

                            displayPlanetStats(newObjects, newObjects.Count - 1, arrayWidth);

                            newObjects = bodyCopy(Objects);
                        }
                        else if (enteredKey.KeyChar == 'a')
                        {
                            Console.Write("  ");
                            if (cursorX - 0.2 > 1)
                            {
                                posX -= posXScroll / 10;
                                jumpOne -= 0.2;
                            }
                            if (jumpOne <= -1)
                            {
                                cursorX -= 2;
                                jumpOne = 1;
                            }
                            newObjects = bodyCopy(Objects);

                            newObjects.Add(addObject(posX, posY, Xvelocity, Yvelocity, mass, radius, name, colour));

                            clearCentredStats(arrayWidth);

                            displayPlanetStats(newObjects, newObjects.Count - 1, arrayWidth);

                            newObjects = bodyCopy(Objects);
                        }
                        else if (enteredKey.KeyChar == 'D')
                        {
                            Console.Write("  ");
                            if (cursorX + 2 < arrayWidth * 2 + 1)
                            {
                                posX += posXScroll;
                                cursorX += 2;
                            }
                            newObjects = bodyCopy(Objects);

                            newObjects.Add(addObject(posX, posY, Xvelocity, Yvelocity, mass, radius, name, colour));

                            clearCentredStats(arrayWidth);

                            displayPlanetStats(newObjects, newObjects.Count - 1, arrayWidth);

                            newObjects = bodyCopy(Objects);
                        }
                        else if (enteredKey.KeyChar == 'd')
                        {
                            Console.Write("  ");
                            if (cursorX + 0.2 < arrayWidth * 2 + 1)
                            {
                                posX += posXScroll / 10;
                                jumpOne += 0.2;
                            }
                            if (jumpOne >= 1)
                            {
                                cursorX += 2;
                                jumpOne = -1;
                            }
                            newObjects = bodyCopy(Objects);

                            newObjects.Add(addObject(posX, posY, Xvelocity, Yvelocity, mass, radius, name, colour));

                            clearCentredStats(arrayWidth);

                            displayPlanetStats(newObjects, newObjects.Count - 1, arrayWidth);

                            newObjects = bodyCopy(Objects);
                        }
                        else if (enteredKey.KeyChar == 'W')
                        {
                            Console.Write("  ");
                            if (cursorY > 1)
                            {
                                posY += posYScroll;
                                cursorY--;
                            }
                            newObjects = bodyCopy(Objects);

                            newObjects.Add(addObject(posX, posY, Xvelocity, Yvelocity, mass, radius, name, colour));

                            clearCentredStats(arrayWidth);

                            displayPlanetStats(newObjects, newObjects.Count - 1, arrayWidth);

                            newObjects = bodyCopy(Objects);
                        }
                        else if (enteredKey.KeyChar == 'w')
                        {
                            Console.Write("  ");
                            if (cursorY - 0.1 > 1)
                            {
                                posY += posYScroll / 10;
                                cursorY -= 0.1;
                            }
                            newObjects = bodyCopy(Objects);

                            newObjects.Add(addObject(posX, posY, Xvelocity, Yvelocity, mass, radius, name, colour));

                            clearCentredStats(arrayWidth);

                            displayPlanetStats(newObjects, newObjects.Count - 1, arrayWidth);

                            newObjects = bodyCopy(Objects);
                        }
                        else if (enteredKey.KeyChar == 'S')
                        {
                            Console.Write("  ");
                            if (cursorY < arrayHeight)
                            {
                                posY -= posYScroll;
                                cursorY++;
                            }
                            newObjects = bodyCopy(Objects);

                            newObjects.Add(addObject(posX, posY, Xvelocity, Yvelocity, mass, radius, name, colour));

                            clearCentredStats(arrayWidth);

                            displayPlanetStats(newObjects, newObjects.Count - 1, arrayWidth);

                            newObjects = bodyCopy(Objects);
                        }
                        else if (enteredKey.KeyChar == 's')
                        {
                            Console.Write("  ");
                            if (cursorY < arrayHeight)
                            {
                                posY -= posYScroll / 10;
                                cursorY += 0.1;
                            }
                            newObjects = bodyCopy(Objects);

                            newObjects.Add(addObject(posX, posY, Xvelocity, Yvelocity, mass, radius, name, colour));

                            clearCentredStats(arrayWidth);

                            displayPlanetStats(newObjects, newObjects.Count - 1, arrayWidth);

                            newObjects = bodyCopy(Objects);
                        }

                        newObjects = bodyCopy(Objects);

                        clearCentredStats(arrayWidth);

                        newObjects.Add(addObject(posX, posY, Xvelocity, Yvelocity, mass, radius, name, colour));

                        displayPlanetStats(newObjects, newObjects.Count - 1, arrayWidth);

                        newObjects = bodyCopy(Objects);

                        Console.CursorTop = (int)Math.Round(cursorY);
                        Console.CursorLeft = (int)Math.Round(cursorX);
                    }
                    else if (adjustingVelocity)
                    {
                        if (enteredKey.KeyChar == 'w')
                        {
                            if (Yvelocity == 0)
                            {
                                Yvelocity = 1;
                            }
                            else if (Yvelocity > -2 && Yvelocity < 0)
                            {
                                Yvelocity = 0;
                            }
                            else if (Yvelocity < 0)
                            {
                                Yvelocity = Yvelocity / 1.05;
                            }
                            else if (Yvelocity > 0)
                            {
                                Yvelocity = Yvelocity * 1.05;
                            }
                        }
                        else if (enteredKey.KeyChar == 'W')
                        {
                            if (Yvelocity == 0)
                            {
                                Yvelocity = 1;
                            }
                            else if (Yvelocity > -2 && Yvelocity < 0)
                            {
                                Yvelocity = 0;
                            }
                            else if (Yvelocity < 0)
                            {
                                Yvelocity = Yvelocity / 2;
                            }
                            else if (Yvelocity > 0)
                            {
                                Yvelocity = Yvelocity * 2;
                            }
                        }
                        else if (enteredKey.KeyChar == 's')
                        {
                            if (Yvelocity == 0)
                            {
                                Yvelocity = -1;
                            }
                            else if (Yvelocity < 2 && Yvelocity > 0)
                            {
                                Yvelocity = 0;
                            }
                            else if (Yvelocity < 0)
                            {
                                Yvelocity = Yvelocity * 1.05;
                            }
                            else if (Yvelocity > 0)
                            {
                                Yvelocity = Yvelocity / 1.05;
                            }
                        }
                        else if (enteredKey.KeyChar == 'S')
                        {
                            if (Yvelocity == 0)
                            {
                                Yvelocity = -1;
                            }
                            else if (Yvelocity < 2 && Yvelocity > 0)
                            {
                                Yvelocity = 0;
                            }
                            else if (Yvelocity < 0)
                            {
                                Yvelocity = Yvelocity * 2;
                            }
                            else if (Yvelocity > 0)
                            {
                                Yvelocity = Yvelocity / 2;
                            }
                        }
                        else if (enteredKey.KeyChar == 'a')
                        {
                            if (Xvelocity == 0)
                            {
                                Xvelocity = -1;
                            }
                            else if (Xvelocity < 2 && Xvelocity > 0)
                            {
                                Xvelocity = 0;
                            }
                            else if (Xvelocity < 0)
                            {
                                Xvelocity = Xvelocity * 1.05;
                            }
                            else if (Xvelocity > 0)
                            {
                                Xvelocity = Xvelocity / 1.05;
                            }
                        }
                        else if (enteredKey.KeyChar == 'A')
                        {
                            if (Xvelocity == 0)
                            {
                                Xvelocity = -1;
                            }
                            else if (Xvelocity < 2 && Xvelocity > 0)
                            {
                                Xvelocity = 0;
                            }
                            else if (Xvelocity < 0)
                            {
                                Xvelocity = Xvelocity * 2;
                            }
                            else if (Xvelocity > 0)
                            {
                                Xvelocity = Xvelocity / 2;
                            }
                        }
                        else if (enteredKey.KeyChar == 'd')
                        {
                            if (Xvelocity == 0)
                            {
                                Xvelocity = 1;
                            }
                            else if (Xvelocity > -2 && Xvelocity < 0)
                            {
                                Xvelocity = 0;
                            }
                            else if (Xvelocity < 0)
                            {
                                Xvelocity = Xvelocity / 1.05;
                            }
                            else if (Xvelocity > 0)
                            {
                                Xvelocity = Xvelocity * 1.05;
                            }
                        }
                        else if (enteredKey.KeyChar == 'D')
                        {
                            if (Xvelocity == 0)
                            {
                                Xvelocity = 1;
                            }
                            else if (Xvelocity > -2 && Xvelocity < 0)
                            {
                                Xvelocity = 0;
                            }
                            else if (Xvelocity < 0)
                            {
                                Xvelocity = Xvelocity / 2;
                            }
                            else if (Xvelocity > 0)
                            {
                                Xvelocity = Xvelocity * 2;
                            }
                        }


                        newObjects = bodyCopy(Objects);

                        clearCentredStats(arrayWidth);

                        newObjects.Add(addObject(posX, posY, Xvelocity, Yvelocity, mass, radius, name, colour));

                        displayPlanetStats(newObjects, newObjects.Count - 1, arrayWidth);

                        newObjects = bodyCopy(Objects);
                    }
                }

                largestDistanceBackup = largestDistance;

                while (simulating)
                {
                    collision = false;
                    Console.CursorVisible = false;
                    Console.CursorLeft = arrayWidth - 6;
                    Console.CursorTop = 1;
                    Console.Write(">SIMULATING<");
                    planetCount = newObjects.Count;

                    while (!Console.KeyAvailable)
                    {

                        newObjects = collisionChecker(newObjects, ref newtrailsQueue, trailLenght, arrayHeight, arrayWidth, ref collision);

                        acceleration = accelerationCalc(newObjects);

                        newObjects = velocityUpdate(newObjects, acceleration, timePerTickInSec);

                        newObjects = positionUpdate(newObjects, timePerTickInSec);

                        if (timeLastImage + timeScale <= time)
                        {
                            if (int.Parse(DateTime.Now.Millisecond.ToString()) < actualTimeMilliseconds + 30 && int.Parse(DateTime.Now.Millisecond.ToString()) > actualTimeMilliseconds - 100)
                            {
                                System.Threading.Thread.Sleep(actualTimeMilliseconds + 30 - int.Parse(DateTime.Now.Millisecond.ToString()));
                            }

                            visualPosition(newObjects, ref largestDistance, ref newtrailsQueue, trailLenght, -1, 0, (int)moveX, (int)moveY, true, arrayHeight, arrayWidth);

                            Console.Write("Current Time Scale: " + secondsSimplifier(timeScale * 30) + " per second     ");
                            Console.Write("\nTime per tick: " + secondsSimplifier(timePerTickInSec) + "     ");

                            if (!collision)
                            {
                                clearCentredStats(arrayWidth);
                                displayPlanetStats(newObjects, planetCount - 1, arrayWidth);
                            }

                            timeLastImage = time;
                            actualTimeMilliseconds = int.Parse(DateTime.Now.Millisecond.ToString());
                        }

                        time += timePerTickInSec;
                    }

                    enteredKey = Console.ReadKey(true);

                    if (enteredKey.Key == ConsoleKey.OemComma)
                    {
                        largestDistance = largestDistance * 1.1;
                        moveX = moveX / 1.1;
                        moveY = moveY / 1.1;
                        //zoom out
                        cleanTrails(ref newtrailsQueue, trailLenght, planetCount);
                        boxCleaner(arrayHeight, arrayWidth);
                    }
                    else if (enteredKey.Key == ConsoleKey.OemPeriod && largestDistance > 100000)
                    {
                        largestDistance = largestDistance / 1.1;
                        moveX = moveX * 1.1;
                        moveY = moveY * 1.1;
                        //zoom in
                        cleanTrails(ref newtrailsQueue, trailLenght, planetCount);
                        boxCleaner(arrayHeight, arrayWidth);
                    }
                    else if (enteredKey.Key == ConsoleKey.RightArrow)
                    {
                        timeChanger(ref timeScale, ref timePerTickInSec, true);
                        //speed up
                    }
                    else if (enteredKey.Key == ConsoleKey.LeftArrow)
                    {
                        timeChanger(ref timeScale, ref timePerTickInSec, false);
                        //slow down
                    }
                    else if (enteredKey.Key == ConsoleKey.Enter)
                    {
                        simulating = false;
                        doneSimulating = true;
                    }
                    else if (enteredKey.Key == ConsoleKey.P)
                    {
                        accept = true;
                        simulating = false;
                        doneSimulating = true;
                    }

                }

                if (doneSimulating)
                {
                    Console.CursorVisible = true;

                    newObjects = bodyCopy(Objects);

                    newObjects.Add(addObject(posX, posY, Xvelocity, Yvelocity, mass, radius, name, colour));

                    clearCentredStats(arrayWidth);
                    displayPlanetStats(newObjects, newObjects.Count - 1, arrayWidth);

                    planetCount = newObjects.Count;

                    boxCleaner(arrayHeight, arrayWidth);

                    Console.CursorLeft = arrayWidth - 8;
                    Console.CursorTop = 1;
                    Console.Write(">ADDING  PLANET<");

                    largestDistance = largestDistanceBackup;

                    doneSimulating = false;
                }

            }

            if (cancel)
            {
                boxCleaner(arrayHeight, arrayWidth);
                clearCentredStats(arrayWidth);
                return Objects;
            }

            visualPosition(newObjects, ref largestDistance, ref newtrailsQueue, trailLenght, -1, 0, (int)moveX, (int)moveY, false, arrayHeight, arrayWidth);

            boxPrinter(5, 20, true, arrayWidth - 21, arrayHeight / 2 - 3);

            Console.CursorLeft = arrayWidth - 9;
            Console.CursorTop = arrayHeight / 2 - 1;
            Console.Write("Enter Planet Name:");
            Console.CursorVisible = false;

            bool takingInput = true;
            name = "";
            string[] colours = new string[] { "red", "dark red", "magenta", "dark magenta", "yellow", "dark yellow", "green", "dark green", "blue", "dark blue", "cyan", "dark cyan", "white" };
            string gap = "";

            for (int i = 0; i < 20; i++)
            {
                gap += " ";
            }
            while (takingInput)
            {
                enteredKey = Console.ReadKey(true);
                if (enteredKey.Key == ConsoleKey.Enter && name.Length >= 2)
                {
                    takingInput = false;
                }
                else if (enteredKey.Key != ConsoleKey.Backspace && enteredKey.Key != ConsoleKey.Enter && enteredKey.KeyChar != '\0' && enteredKey.KeyChar != '\t' && name.Length < 18)
                {
                    name += enteredKey.KeyChar;
                    gap = gap.Substring(gap.Length - 1);
                }
                else if (enteredKey.Key == ConsoleKey.Backspace)
                {
                    if (name.Length > 0)
                    {
                        name = name.Substring(0, name.Length - 1);
                        gap += " ";
                    }
                }

                Console.CursorLeft = arrayWidth - 9;
                Console.CursorTop = arrayHeight / 2 + 1;
                Console.Write(name);
                Console.Write(gap);
            }

            Console.CursorLeft = arrayWidth - 9;
            Console.CursorTop = arrayHeight / 2 - 1;
            Console.Write("Select Planet Colour:");
            Console.CursorVisible = false;

            int colourID = 0;
            takingInput = true;

            while (takingInput)
            {
                Console.CursorLeft = arrayWidth - 9;
                Console.CursorTop = arrayHeight / 2 + 1;

                Console.Write("← ");

                colourChanger(colours[colourID]);

                Console.Write(name);

                Console.ForegroundColor = ConsoleColor.Gray;

                Console.Write(" →");

                enteredKey = Console.ReadKey(true);

                if (enteredKey.Key == ConsoleKey.Enter)
                {
                    colour = colours[colourID];

                    takingInput = false;
                }
                else if (enteredKey.Key == ConsoleKey.LeftArrow)
                {
                    if (colourID - 1 > 0)
                    {
                        colourID--;
                    }
                    else
                    {
                        colourID = colours.Length - 1;
                    }
                }
                else if (enteredKey.Key == ConsoleKey.RightArrow)
                {
                    if (colourID + 1 < colours.Length)
                    {
                        colourID++;
                    }
                    else
                    {
                        colourID = 0;
                    }
                }

            }

            newObjects = bodyCopy(Objects);
            newPlanet = addObject(posX, posY, Xvelocity, Yvelocity, mass, radius, name, colour);
            newObjects.Add(newPlanet);
            Console.Clear();

            return newObjects;
        }

        static List<Body> collision(List<Body> Objects, int object1, int object2)
        {
            List<Body> newObjects = new List<Body>();
            Body resultantObject;

            double newX = (Objects[object1].posX + Objects[object2].posX) / 2;

            double newY = (Objects[object1].posY + Objects[object2].posY) / 2;

            double newMass = Objects[object1].mass + Objects[object2].mass;

            double newXVelocity = Objects[object1].xVelocity * Objects[object1].mass + Objects[object2].xVelocity * Objects[object2].mass;
            //energy = mass * velocity
            double newYVelocity = Objects[object1].yVelocity * Objects[object1].mass + Objects[object2].yVelocity * Objects[object2].mass;

            newXVelocity = newXVelocity / newMass;
            newYVelocity = newYVelocity / newMass;

            //energy / mass = velocity

            double newRadius;
            newRadius = Objects[object1].radius * Objects[object1].radius * 3.141 + Objects[object2].radius * Objects[object2].radius * 3.141; //total volume
            newRadius = Math.Sqrt(newRadius / 3.141);

            string newName;
            string newColour;
            int newID;
            int removedID;
            if (Objects[object1].mass > Objects[object2].mass)
            {
                newName = Objects[object1].name;
                newColour = Objects[object1].colour;
                newID = object1;
                removedID = object2;
            }
            else
            {
                newName = Objects[object2].name;
                newColour = Objects[object2].colour;
                newID = object2;
                removedID = object1;
            }

            resultantObject = addObject(newX, newY, newXVelocity, newYVelocity, newMass, newRadius, newName, newColour);

            for (int i = 0; i < Objects.Count; i++)
            {
                if (i != removedID)
                {
                    if (i == newID)
                    {
                        newObjects.Add(resultantObject);
                    }
                    else
                    {
                        newObjects.Add(Objects[i]);
                    }
                }
            }

            return newObjects;
        }

        static List<Body> collisionChecker(List<Body> Objects, ref List<string[,]> trailsQueue, int trailLenght, int arrayHeight, int arrayWidth, ref bool anyCollision)
        {
            List<double[]> line1 = new List<double[]> { };
            List<double[]> line2 = new List<double[]> { };
            double[] holerPoint = new double[] { 0, 0 };
            line1.Add(holerPoint);
            line1.Add(holerPoint);
            line2.Add(holerPoint);
            line2.Add(holerPoint);

            bool collisionOccured = false;
            double distance;
            for (int i = 0; i < Objects.Count - 1; i++)
            {
                for (int j = i + 1; j < Objects.Count; j++)
                {
                    distance = distanceCalc(Objects[i].posX, Objects[i].posY, Objects[j].posX, Objects[j].posY);

                    if (Objects[i].radius + Objects[j].radius > distance)
                    {
                        collisionOccured = true;
                        anyCollision = true;
                    }
                    else
                    {
                        line1[0][0] = Objects[i].posX;
                        line1[0][1] = Objects[i].posY;
                        line1[1][0] = Objects[i].posX - Objects[i].xVelocity;
                        line1[1][1] = Objects[i].posY - Objects[i].yVelocity;

                        line2[0][0] = Objects[j].posX;
                        line2[0][1] = Objects[j].posY;
                        line2[1][0] = Objects[j].posX - Objects[j].xVelocity;
                        line2[1][1] = Objects[j].posY - Objects[j].yVelocity;

                        if (lineOverlapCheck(line1, line2))
                        {
                            collisionOccured = true;
                            anyCollision = true;
                        }
                    }
                    if (collisionOccured)
                    {
                        Objects = collision(Objects, i, j);
                        trailsQueue = trailQueueAdd(trailsQueue, trailLenght, Objects);
                        boxCleaner(arrayHeight, arrayWidth);
                        collisionOccured = false;
                    }
                }
            }
            return Objects;
        }

        static bool lineOverlapCheck(List<double[]> line1, List<double[]> line2)
        {
            bool overlap = false;
            if (arePointAntiClockwise(line1[0], line2[0], line2[1]) != arePointAntiClockwise(line1[1], line2[0], line2[1]) && arePointAntiClockwise(line1[0], line1[1], line2[0]) != arePointAntiClockwise(line1[0], line1[1], line2[1]))
            {
                overlap = true;
            }
            else
            {
                overlap = false;
            }
            return overlap;
        }
        //using "algorithm" from https://bryceboe.com/2006/10/23/line-segment-intersection-algorithm/
        static bool arePointAntiClockwise(double[] point1, double[] point2, double[] point3)
        {
            bool isAntiClockwise;
            if ((point3[1] - point1[1]) * (point2[0] - point1[0]) > (point2[1] - point1[1]) * (point3[0] - point1[0]))
            {
                isAntiClockwise = true;
            }
            else
            {
                isAntiClockwise = false;
            }
            return isAntiClockwise;
        }

        static string secondsSimplifier(int seconds)
        {
            int minutes;
            int hours;
            int days;
            int weeks;
            if (seconds % 60 == 0)
            {
                minutes = seconds / 60;
                if (minutes % 60 == 0)
                {
                    hours = minutes / 60;
                    if (hours % 24 == 0)
                    {
                        days = hours / 24;
                        if (Math.Round(days / 365.25) > 1)
                        {
                            return Math.Round(days / 365.25) + " years";
                        }
                        if (days % 7 == 0)
                        {
                            weeks = days / 7;
                            if (weeks > 1)
                            {
                                return weeks + " weeks";
                            }
                            else
                            {
                                return weeks + " week";
                            }
                        }
                        if (days > 1)
                        {
                            return days + " days";
                        }
                        else
                        {
                            return days + " day";
                        }
                    }
                    if (hours > 1)
                    {
                        return hours + " hours"; ;
                    }
                    else
                    {
                        return hours + " hour";
                    }
                }
                if (minutes > 1)
                {
                    return minutes + " minutes";
                }
                else
                {
                    return minutes + " minute";
                }
            }
            if (seconds > 1)
            {
                return seconds + " seconds";
            }
            else
            {
                return seconds + " seconds";
            }


        }

        static void timeChanger(ref int timeScale, ref int timePerTickInSec, bool increase)
        {
            int[] timeScales = new int[] { 1, 5, 10, 30, 60, 120, 600, 1800, 3600, 7200, 14400, 43200, 86400, 172800, 345600, 604800, 1209600, 2419200 };
            //timeScales: 1s, 5s, 10s, 30s, 1min, 2mins, 10mins, 30mins, 1hour, 2hours, 4hours, 1day, 2days, 4days, 1week, 2weeks, 4weeks
            int scalePosition;
            int tickPosition;
            for (scalePosition = 0; scalePosition < timeScales.Length; scalePosition++)
            {
                if (timeScale == timeScales[scalePosition])
                {
                    break;
                }
            }
            for (tickPosition = 0; tickPosition < timeScales.Length; tickPosition++)
            {
                if (timePerTickInSec == timeScales[tickPosition])
                {
                    break;
                }
            }
            if (increase == true && scalePosition < timeScales.Count() - 1)
            {
                timeScale = timeScales[scalePosition + 1];
                if (tickPosition < scalePosition - 4 && timeScales[tickPosition + 1] <= 60 * 60)
                {
                    timePerTickInSec = timeScales[tickPosition + 1];
                }
            }

            else if (increase == false && scalePosition > 0)
            {
                timeScale = timeScales[scalePosition - 1];

                if (tickPosition > scalePosition - 6 && tickPosition > 0)
                {
                    timePerTickInSec = timeScales[tickPosition - 1];
                }
            }
        }

        static List<Body> objectsImporter(List<string> input)
        {
            List<Body> output;
            try
            {
                output = new List<Body> { };
                string[] splitInput2 = new string[8];

                for (int i = 0; i < input.Count(); i++)
                {
                    splitInput2 = input[i].Split(',');
                    output.Add(addObject(Convert.ToDouble(splitInput2[0]), Convert.ToDouble(splitInput2[1]), Convert.ToDouble(splitInput2[2]), Convert.ToDouble(splitInput2[3]), Convert.ToDouble(splitInput2[4]), Convert.ToDouble(splitInput2[5]), splitInput2[6], splitInput2[7]));
                }
                return output;
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine("Input error, check that you have copied the text correctly");
                Console.WriteLine("Enter any key to return");
                Console.ReadKey(true);
                return null;
            }
        }

        static string objectsExporter(List<Body> input)
        {
            string output = "";
            for (int i = 0; i < input.Count(); i++)
            {
                output += input[i].posX.ToString() + ",";
                output += input[i].posY.ToString() + ",";
                output += input[i].xVelocity.ToString() + ",";
                output += input[i].yVelocity.ToString() + ",";
                output += input[i].mass.ToString() + ",";
                output += input[i].radius.ToString() + ",";
                output += input[i].name + ",";
                output += input[i].colour;
                output += "\n";
            }
            return output;
        }

        static void Main(string[] args)
        {
            List<Body> solarSystem = new List<Body>();
            List<Body> extraObjects = new List<Body>();
            List<Body> Objects = new List<Body>();

            solarSystem.Add(addObject(0, 0, 0, -0.19, 1.989e30, 696340000, "Sun", "yellow"));
            solarSystem.Add(addObject(5.791e10, 0, 0, 47.36e3, 3.285e23, 2439700, "Mercury", "gray"));
            solarSystem.Add(addObject(1.082e11, 0, 0, 35.02e3, 4.867e24, 6051800, "Venus", "gray"));
            solarSystem.Add(addObject(1.496e11, 0, 0, 29.78e3, 5.972e24, 6371000, "Earth", "green"));
            solarSystem.Add(addObject(2.279e11, 0, 0, 24.07e3, 6.39e23, 3396200, "Mars", "red"));

            solarSystem.Add(addObject(7.785e11, 0, 0, 13e3, 1.898e27, 69911000, "Jupiter", "dark yellow"));
            solarSystem.Add(addObject(1.434e12, 0, 0, 9.68e3, 5.683e26, 58232000, "Saturn", "dark yellow"));
            solarSystem.Add(addObject(2.871e12, 0, 0, 6.80e3, 8.681e25, 25362000, "Uranus", "dark cyan"));
            solarSystem.Add(addObject(4.495e12, 0, 0, 5.43e3, 1.024e26, 24622000, "Neptune", "dark blue"));

            extraObjects.Add(addObject(-2e9, 0, 0, 9.108e4, 9.945e29, 74085000, "1Sun", "yellow"));
            extraObjects.Add(addObject(2e9, 0, 0, -9.108e4, 9.945e29, 74085000, "2Sun", "yellow"));

            int planetCount = Objects.Count;
            ConsoleKey enteredKey = ConsoleKey.L;
            string input;
            List<string> newObjects = new List<string>();

            double[,] acceleration;
            bool simulationFinished = false;
            bool paused = false;
            bool alreadyPaused = false;
            bool showingInstructions = true;
            bool collision = false;
            bool justOpened = true;
            int centredType = 0;

            int arrayHeight = Console.LargestWindowHeight - 4;
            int arrayWidth = (Console.LargestWindowWidth - 50) / 2 - 2;

            //int arrayHeight = 50;
            //int arrayWidth = 30;

            Console.WindowHeight = Console.LargestWindowHeight;
            Console.WindowWidth = Console.LargestWindowWidth;

            int centredPlanet = -1;
            int minCentred = -1;

            double moveX = 0;
            double moveY = 0;

            double largestDistance = largestDistanceFromCenter(Objects) * 1.1;
            //largest distance that needs to be considered when displaying image

            double time = 1;
            double timeLastImage = 1;

            int timeScale = 1;
            int timePerTickInSec = 1;

            int actualTimeMilliseconds = int.Parse(DateTime.Now.Millisecond.ToString());

            List<string[,]> trailsQueue = new List<string[,]> { };
            int trailLenght = 13;

            trailsQueue = trailQueueAdd(trailsQueue, trailLenght, Objects);

            Console.WriteLine("Starting");
            System.Threading.Thread.Sleep(500);

            Console.Clear();

            Console.CursorVisible = false;
            Console.Title = "NEA";

            while (!simulationFinished)
            {
                if (!justOpened)
                {
                    while (!Console.KeyAvailable)
                    {

                        Objects = collisionChecker(Objects, ref trailsQueue, trailLenght, arrayHeight, arrayWidth, ref collision);
                        //Checks if there are two bodies that overlap

                        acceleration = accelerationCalc(Objects);
                        //accelerations stored as (object, 0 and 1), where object referes to the number that body is in the Objects array and 0 being x axis acceleration or 1 being y axis acceleration

                        Objects = velocityUpdate(Objects, acceleration, timePerTickInSec);
                        //update the velocity of objects using the acceleration found

                        Objects = positionUpdate(Objects, timePerTickInSec);
                        //update the velocity of objects using the velocities

                        if (timeLastImage + timeScale <= time)
                        {
                            if (int.Parse(DateTime.Now.Millisecond.ToString()) < actualTimeMilliseconds + 30 && int.Parse(DateTime.Now.Millisecond.ToString()) > actualTimeMilliseconds - 100)
                            {
                                System.Threading.Thread.Sleep(actualTimeMilliseconds + 30 - int.Parse(DateTime.Now.Millisecond.ToString()));
                            }

                            visualPosition(Objects, ref largestDistance, ref trailsQueue, trailLenght, centredPlanet, centredType, (int)moveX, (int)moveY, true, arrayHeight, arrayWidth);
                            Console.Write("Current Time Scale: " + secondsSimplifier(timeScale) + " per frame,  time per tick: " + timePerTickInSec + ",  time per second: " + secondsSimplifier(timeScale * 30) + "    ");
                            Console.Write("\nTime per tick: " + secondsSimplifier(timePerTickInSec) + "     ");

                            if (centredPlanet != -1)
                            {
                                clearCentredStats(arrayWidth);
                                displayPlanetStats(Objects, centredPlanet, arrayWidth);
                            }

                            timeLastImage = time;
                            actualTimeMilliseconds = int.Parse(DateTime.Now.Millisecond.ToString());
                        }

                        time += timePerTickInSec;
                    }

                    enteredKey = Console.ReadKey(true).Key;
                }

                planetCount = Objects.Count;

                if (enteredKey == ConsoleKey.Spacebar)
                {
                    paused = true;
                }
                else if (enteredKey == ConsoleKey.LeftArrow)
                {
                    timeChanger(ref timeScale, ref timePerTickInSec, false);
                    //speed down
                }
                else if (enteredKey == ConsoleKey.RightArrow)
                {
                    if (largestDistanceFromCenter(Objects) > 1e9)
                    {
                        timeChanger(ref timeScale, ref timePerTickInSec, true);
                        //speed up
                    }
                    else if (largestDistanceFromCenter(Objects) < 1e9 && timePerTickInSec < 60)
                    {
                        timeChanger(ref timeScale, ref timePerTickInSec, true);
                    }
                }
                else if (enteredKey == ConsoleKey.OemComma)
                {
                    largestDistance = largestDistance * 1.1;
                    moveX = moveX / 1.1;
                    moveY = moveY / 1.1;
                    //zoom out
                    boxCleaner(arrayHeight, arrayWidth);
                    cleanTrails(ref trailsQueue, trailLenght, planetCount);
                    visualPosition(Objects, ref largestDistance, ref trailsQueue, trailLenght, centredPlanet, centredType, (int)moveX, (int)moveY, true, arrayHeight, arrayWidth);
                }
                else if (enteredKey == ConsoleKey.OemPeriod && largestDistance > 10)
                {
                    largestDistance = largestDistance / 1.1;
                    moveX = moveX * 1.1;
                    moveY = moveY * 1.1;
                    //zoom in
                    boxCleaner(arrayHeight, arrayWidth);
                    cleanTrails(ref trailsQueue, trailLenght, planetCount);
                    visualPosition(Objects, ref largestDistance, ref trailsQueue, trailLenght, centredPlanet, centredType, (int)moveX, (int)moveY, true, arrayHeight, arrayWidth);
                }
                else if (enteredKey == ConsoleKey.L)
                {
                    Console.CursorVisible = true;
                    Console.Clear();
                    if (!justOpened)
                    {
                        Console.WriteLine("(E)xporting current system, (I)mporting something new or (L)oading a preset? (C) to cancel");
                        enteredKey = Console.ReadKey(true).Key;
                        while (enteredKey != ConsoleKey.E && enteredKey != ConsoleKey.I && enteredKey != ConsoleKey.L && enteredKey != ConsoleKey.C)
                        {
                            enteredKey = Console.ReadKey(true).Key;
                        }
                    }
                    else
                    {
                        Console.WriteLine("(I)mporting something or (L)oading a preset?");
                        enteredKey = Console.ReadKey(true).Key;
                        while (enteredKey != ConsoleKey.I && enteredKey != ConsoleKey.L)
                        {
                            enteredKey = Console.ReadKey(true).Key;
                        }
                    }
                    if (enteredKey == ConsoleKey.E)
                    {
                        Console.Clear();
                        Console.Write(objectsExporter(Objects));
                        Console.Write("\n\nCopy text then enter R to return to simulation.");
                        enteredKey = Console.ReadKey(true).Key;
                        while (enteredKey != ConsoleKey.R)
                        {
                            enteredKey = Console.ReadKey(true).Key;
                        }
                    }
                    else if (enteredKey == ConsoleKey.I)
                    {
                        Console.Write("Enter import text. (C) to finish:\n");
                        input = "";
                        newObjects = new List<string> { };
                        while (true)
                        {
                            input = Console.ReadLine();
                            if (input.ToLower() != "c")
                            {
                                newObjects.Add(input);
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (objectsImporter(newObjects) != null)
                        {
                            if (newObjects.Count != 0)
                            {
                                Objects = objectsImporter(newObjects);
                                timeScale = 1;
                                timePerTickInSec = 1;
                                trailsQueue = new List<string[,]>();
                                trailsQueue = trailQueueAdd(trailsQueue, trailLenght, Objects);
                                largestDistance = largestDistanceFromCenter(Objects) * 1.1;


                                justOpened = false;
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Input error, check that you have copied the text correctly");
                                Console.WriteLine("Enter any key to return");
                                Console.ReadKey(true);
                                enteredKey = ConsoleKey.L;
                            }
                        }
                        else
                        {
                            enteredKey = ConsoleKey.L;
                        }
                    }
                    else if (enteredKey == ConsoleKey.L)
                    {
                        Console.Clear();
                        Console.Write("Presets:");
                        Console.Write("\n1) inner solar system\n2) outer solar system\n3) whole solar system\n4) binary stars\n5) solar system with binary stars");
                        enteredKey = Console.ReadKey(true).Key;
                        while (enteredKey != ConsoleKey.D1 && enteredKey != ConsoleKey.D2 && enteredKey != ConsoleKey.D3 && enteredKey != ConsoleKey.D4 && enteredKey != ConsoleKey.D5 && enteredKey != ConsoleKey.C)
                        {
                            enteredKey = Console.ReadKey(true).Key;
                        }
                        if (enteredKey == ConsoleKey.D1)
                        {
                            Objects = new List<Body> { };
                            for (int i = 0; i < 5; i++)
                            {
                                Objects.Add(solarSystem[i]);
                            }
                        }
                        else if (enteredKey == ConsoleKey.D2)
                        {
                            Objects = new List<Body> { };
                            Objects.Add(solarSystem[0]);
                            for (int i = 5; i < 9; i++)
                            {
                                Objects.Add(solarSystem[i]);
                            }
                        }
                        else if (enteredKey == ConsoleKey.D3)
                        {
                            Objects = new List<Body> { };
                            for (int i = 0; i < solarSystem.Count; i++)
                            {
                                Objects.Add(solarSystem[i]);
                            }
                        }
                        else if (enteredKey == ConsoleKey.D4)
                        {
                            Objects = new List<Body> { };
                            Objects.Add(extraObjects[0]);
                            Objects.Add(extraObjects[1]);
                        }
                        else if (enteredKey == ConsoleKey.D5)
                        {
                            Objects = new List<Body> { };
                            Objects.Add(extraObjects[0]);
                            Objects.Add(extraObjects[1]);
                            for (int i = 1; i < solarSystem.Count; i++)
                            {
                                Objects.Add(solarSystem[i]);
                            }
                        }
                        justOpened = false;
                        timeScale = 1;
                        timePerTickInSec = 1;
                        trailsQueue = new List<string[,]>();
                        trailsQueue = trailQueueAdd(trailsQueue, trailLenght, Objects);
                        largestDistance = largestDistanceFromCenter(Objects) * 1.1;
                    }
                    Console.CursorVisible = false;
                    Console.Clear();
                    boxPrinter(arrayHeight, arrayWidth);
                    printInstructions(showingInstructions, arrayWidth);
                }
                else if (enteredKey == ConsoleKey.W)
                {
                    moveY++;
                    boxCleaner(arrayHeight, arrayWidth);
                    visualPosition(Objects, ref largestDistance, ref trailsQueue, trailLenght, centredPlanet, centredType, (int)moveX, (int)moveY, true, arrayHeight, arrayWidth);
                }
                else if (enteredKey == ConsoleKey.S)
                {
                    moveY--;
                    boxCleaner(arrayHeight, arrayWidth);
                    visualPosition(Objects, ref largestDistance, ref trailsQueue, trailLenght, centredPlanet, centredType, (int)moveX, (int)moveY, true, arrayHeight, arrayWidth);
                }
                else if (enteredKey == ConsoleKey.A)
                {
                    moveX += 2;
                    boxCleaner(arrayHeight, arrayWidth);
                    visualPosition(Objects, ref largestDistance, ref trailsQueue, trailLenght, centredPlanet, centredType, (int)moveX, (int)moveY, true, arrayHeight, arrayWidth);
                }
                else if (enteredKey == ConsoleKey.D)
                {
                    moveX -= 2;
                    boxCleaner(arrayHeight, arrayWidth);
                    visualPosition(Objects, ref largestDistance, ref trailsQueue, trailLenght, centredPlanet, centredType, (int)moveX, (int)moveY, true, arrayHeight, arrayWidth);
                }
                else if (enteredKey == ConsoleKey.R)
                {
                    moveX = 0;
                    moveY = 0;
                    largestDistance = largestDistanceFromCenter(Objects) * 1.1;
                    boxCleaner(arrayHeight, arrayWidth);
                    cleanTrails(ref trailsQueue, trailLenght, planetCount);
                    visualPosition(Objects, ref largestDistance, ref trailsQueue, trailLenght, centredPlanet, centredType, (int)moveX, (int)moveY, true, arrayHeight, arrayWidth);
                }
                else if (enteredKey == ConsoleKey.I)
                {
                    showingInstructions = !showingInstructions;
                    if (showingInstructions)
                    {
                        Console.Clear();
                        arrayWidth = (Console.WindowWidth - 50) / 2 - 2;
                    }
                    else
                    {
                        arrayWidth = (Console.WindowWidth - 30) / 2 - 2;
                    }
                    printInstructions(showingInstructions, arrayWidth);
                    cleanTrails(ref trailsQueue, trailLenght, planetCount);
                    boxCleaner(arrayHeight, arrayWidth);
                    boxPrinter(arrayHeight, arrayWidth);
                }
                else if (enteredKey == ConsoleKey.P)
                {
                    Objects = addPlanet(Objects, arrayHeight, arrayWidth, largestDistance, trailsQueue, trailLenght, moveX, moveY);
                    //Objects = new List<Body>();
                    //Objects = holder;
                    //holder = new List<Body>();
                    trailsQueue = trailQueueAdd(trailsQueue, trailLenght, Objects);
                    printInstructions(showingInstructions, arrayWidth);
                    boxCleaner(arrayHeight, arrayWidth);
                    boxPrinter(arrayHeight, arrayWidth);
                }
                else if (enteredKey == ConsoleKey.Q)
                {
                    centredPlanet--;
                    if (centredPlanet < minCentred)
                    {
                        centredPlanet = planetCount - 1;
                    }
                    //cycle planets backwards

                    if (centredType != 0)
                    {
                        cleanTrails(ref trailsQueue, trailLenght, planetCount);
                        boxCleaner(arrayHeight, arrayWidth);
                        moveX = 0;
                        moveY = 0;
                    }
                    else
                    {
                        clearCentredStats(arrayWidth);
                    }
                }
                else if (enteredKey == ConsoleKey.E)
                {
                    centredPlanet++;
                    if (centredPlanet >= planetCount)
                    {
                        centredPlanet = minCentred;
                    }
                    //cycle planets forwards

                    if (centredType != 0)
                    {
                        cleanTrails(ref trailsQueue, trailLenght, planetCount);
                        boxCleaner(arrayHeight, arrayWidth);
                        moveX = 0;
                        moveY = 0;
                    }
                    else
                    {
                        clearCentredStats(arrayWidth);
                    }
                }
                else if (enteredKey == ConsoleKey.C)
                {
                    //cycle centre view type
                    if (centredPlanet > -1)
                    {
                        if (centredType < 2)
                        {
                            centredType++;
                            minCentred = 0;
                            if (centredType == 2)
                            {
                                boxCleaner(arrayHeight, arrayWidth);
                                cleanTrails(ref trailsQueue, trailLenght, planetCount);
                            }
                        }
                        else
                        {
                            centredType = 0;
                            moveX = 0;
                            moveY = 0;
                            minCentred = -1;
                            boxCleaner(arrayHeight, arrayWidth);
                            cleanTrails(ref trailsQueue, trailLenght, planetCount);
                        }
                    }
                    printInstructions(showingInstructions, arrayWidth);
                }
                else if (enteredKey == ConsoleKey.X)
                {
                    boxPrinter(5, 20, true, arrayWidth - 21, arrayHeight / 2 - 3);

                    Console.CursorLeft = arrayWidth - 9;
                    Console.CursorTop = arrayHeight / 2 - 2;
                    Console.Write("Removing:");

                    Console.CursorLeft = arrayWidth - 9;
                    Console.CursorTop = arrayHeight / 2 - 1;
                    Console.Write(Objects[centredPlanet].name);

                    Console.CursorLeft = arrayWidth - 9;
                    Console.CursorTop = arrayHeight / 2;
                    Console.Write("Are you sure?(Y/N)");

                    if (Console.ReadKey().Key == ConsoleKey.Y)
                    {
                        Objects = planetRemover(Objects, centredPlanet);
                        centredPlanet = -1;
                        trailsQueue = trailQueueAdd(trailsQueue, trailLenght, Objects);
                        boxCleaner(arrayHeight, arrayWidth);
                        clearCentredStats(arrayWidth);
                    }

                }
                while (paused)
                {
                    if (alreadyPaused == false)
                    {
                        Console.CursorLeft = arrayWidth - 4;
                        Console.CursorTop = 1;
                        Console.Write(">PAUSED<");
                        Console.CursorLeft = 0;
                        Console.CursorTop = arrayHeight + 4;
                        alreadyPaused = true;
                    }

                    enteredKey = Console.ReadKey(true).Key;

                    if (enteredKey == ConsoleKey.Spacebar)
                    {
                        paused = false;
                        alreadyPaused = false;
                        Console.CursorLeft = arrayWidth - 4;
                        Console.CursorTop = 1;
                        Console.Write("        ");
                    }
                    System.Threading.Thread.Sleep(50);
                }
                System.Threading.Thread.Sleep(30);
            }
        }

        static void displayPlanetStats(List<Body> Objects, int centredPlanet, int arrayWidth)
        {
            int row = 0;
            scroll(arrayWidth, row);
            Console.Write("Centred Planet:");
            row++;
            scroll(arrayWidth, row);
            Console.Write("Name: " + Objects[centredPlanet].name);
            row++;
            scroll(arrayWidth, row);
            Console.Write("X: " + Math.Round(Objects[centredPlanet].posX) + "m");
            row++;
            scroll(arrayWidth, row);
            Console.Write("Y: " + Math.Round(Objects[centredPlanet].posY) + "m");
            row++;
            scroll(arrayWidth, row);
            Console.Write("X Velocity: " + Math.Round(Objects[centredPlanet].xVelocity, 3) + "m/s");
            row++;
            scroll(arrayWidth, row);
            Console.Write("Y Velocity: " + Math.Round(Objects[centredPlanet].yVelocity, 3) + "m/s");
            row++;
            scroll(arrayWidth, row);
            Console.Write("Mass: " + Objects[centredPlanet].mass + "kg");
            row++;
            scroll(arrayWidth, row);
            Console.Write("Radius: " + Objects[centredPlanet].radius + "m");
        }

        static void clearCentredStats(int arrayWidth)
        {
            string clear = "";
            for (int i = 0; i < Console.WindowWidth - (arrayWidth * 2 + 2); i++)
            {
                clear += " ";
            }
            for (int row = 0; row < 8; row++)
            {
                scroll(arrayWidth, row);
                Console.Write(clear);
            }
        }

        static void printInstructions(bool open, int arrayWidth,
            bool forPlanetAdder = false, bool editingVelocity = false)
        {
            int row = 8;
            string spacer = "";
            string whatIsEdited;
            for (int i = 0; i < (Console.WindowWidth - (arrayWidth * 2 + 2) - 12) / 2 - 2; i++)
            {
                spacer += "-";
            }
            scroll(arrayWidth, row);
            Console.Write(spacer + "INSTRUCTIONS" + spacer);
            row++;
            if (open)
            {
                if (forPlanetAdder)
                {
                    if (editingVelocity)
                    {
                        whatIsEdited = "velocity";
                    }
                    else
                    {
                        whatIsEdited = "position";
                    }
                    for (int i = 0; i < Console.WindowWidth - (arrayWidth * 2 + 2); i++)
                    {
                        spacer += "-";
                    }
                    scroll(arrayWidth, row);
                    Console.Write("WASD to edit the planets' " + whatIsEdited + " quickly");
                    row++;
                    scroll(arrayWidth, row);
                    Console.Write("wasd to edit the planets' " + whatIsEdited + " accurately");
                    row++;
                    scroll(arrayWidth, row);
                    Console.Write("SPACE to switch editing velocity and position");
                    row++;
                    scroll(arrayWidth, row);
                    Console.Write("UP/DOWN ARROW to edit mass");
                    row++;
                    scroll(arrayWidth, row);
                    Console.Write("LEFT/RIGHT ARROW to edit radius");
                    row++;
                    scroll(arrayWidth, row);
                    Console.Write("C to cancel");
                    row++;
                    scroll(arrayWidth, row);
                    Console.Write("ENTER to start / reset simulation");
                    row++;
                    scroll(arrayWidth, row);
                    Console.Write("< and > to zoom in/out");
                    row++;
                    scroll(arrayWidth, row);
                    Console.Write("LEFT/RIGHT ARROW to change the simulation speed");
                }
                else
                {
                    scroll(arrayWidth, row);
                    Console.Write("WASD to move camera");
                    row++;
                    scroll(arrayWidth, row);
                    Console.Write("< > to zoome in/out");
                    row++;
                    scroll(arrayWidth, row);
                    Console.Write("LEFT/RIGHT ARROW to change simulation speed");
                    row++;
                    scroll(arrayWidth, row);
                    Console.Write("P to add planet");
                    row++;
                    scroll(arrayWidth, row);
                    Console.Write("Q and E to select planets");
                    row++;
                    scroll(arrayWidth, row);
                    Console.Write("C to centre on selected planet");
                    row++;
                    scroll(arrayWidth, row);
                    Console.Write("R to reset camera");
                    row++;
                    scroll(arrayWidth, row);
                    Console.Write("SPACE to pause");
                    row++;
                    scroll(arrayWidth, row);
                    Console.Write("X to remove selected planet");
                    row++;
                    scroll(arrayWidth, row);
                    Console.Write("L to load/import/export systems");
                    row++;
                    scroll(arrayWidth, row);
                    Console.Write("I to hide this menu");
                }
            }
            else
            {
                string clear = "";
                for (int i = 0; i < Console.WindowWidth - (arrayWidth * 2 + 2); i++)
                {
                    clear += " ";
                }
                for (row = 9; row < 20; row++)
                {
                    scroll(arrayWidth, row);
                    Console.Write(clear);
                }
            }
        }

        static void boxPrinter(int arrayHeight, int arrayWidth,
            bool empty = false, int x = 0, int y = 0)
        {
            string characterCounter = "";
            for (int i = 0; i < (arrayWidth + 1) * 2; i++)
            {
                characterCounter += "-";
            }

            Console.CursorLeft = x;
            Console.CursorTop = y;
            Console.Write(characterCounter);
            Console.CursorTop = y + 1;
            for (int i = 0; i < arrayHeight + 1; i++)
            {
                Console.CursorLeft = x;
                Console.Write("|");
                if (empty)
                {
                    for (int j = 0; j < arrayWidth * 2; j++)
                    {
                        Console.Write(" ");
                    }
                }
                Console.CursorLeft = x + arrayWidth * 2 + 1;
                Console.Write("|");
                Console.CursorTop = y + i + 1;
            }
            Console.CursorLeft = x;
            Console.WriteLine(characterCounter);
        }

        static void boxCleaner(int arrayHeight, int arrayWidth)
        {
            string clear = "";
            for (int i = 0; i < arrayWidth * 2; i++)
            {
                clear += " ";
            }
            for (int i = 1; i < arrayHeight + 1; i++)
            {
                Console.CursorLeft = 1;
                Console.CursorTop = i;
                Console.Write(clear);
            }
        }

        static void cursorArrayPrinter(List<Body> Objects, int[,] xyPos, int moveX, int moveY, int arrayHeight, int arrayWidth)
        {
            //boxPrinter(arrayHeight, arrayWidth);

            for (int i = 0; i < Objects.Count; i++)
            {
                if (xyPos[i, 0] != -1)
                {
                    if (xyPos[i, 1] * 2 + 1 + moveX < arrayWidth * 2 && xyPos[i, 2] + moveY < arrayHeight)
                    {
                        if (xyPos[i, 1] * 2 + 1 + moveX > 0 && xyPos[i, 2] + 1 + moveY > 0)
                        {
                            Console.CursorLeft = xyPos[i, 1] * 2 + 1 + moveX;
                            Console.CursorTop = xyPos[i, 2] + 1 + moveY;
                            colourChanger(Objects[xyPos[i, 0]].colour);
                            Console.Write(Objects[xyPos[i, 0]].name.Substring(0, 2));
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                    }
                }
            }
            Console.CursorTop = arrayHeight + 2;
            Console.CursorLeft = 0;
        }

        static void cycleTrailQueue(int posX, int posY, ref List<string[,]> trailsQueue, string BodyName, int trailLenght, int moveX, int moveY, int arrayHeight, int arrayWidth)
        {
            for (int i = 0; i < trailsQueue.Count; i++)
            {
                if (BodyName == trailsQueue[i][0, 0])
                {
                    for (int j = 0; j < trailLenght; j++)
                    {
                        if (trailsQueue[i][j, 3] == null)
                        {
                            if (posX * 2 + 1 + moveX < arrayWidth * 2 && posY + moveY < arrayHeight)
                            {
                                if (posX + 1 + moveX > 0 && posY + 1 + moveY > 0)
                                {
                                    trailsQueue[i][j, 1] = posX.ToString();
                                    trailsQueue[i][j, 2] = posY.ToString();
                                    trailsQueue[i][j, 3] = trailLenght.ToString();
                                }
                            }
                            break;
                        }
                        else
                        {
                            if (int.Parse(trailsQueue[i][j, 3]) <= 0)
                            {
                                if (int.Parse(trailsQueue[i][j, 1]) * 2 + 1 + moveX > 0 && int.Parse(trailsQueue[i][j, 2]) + 1 + moveY > 0)
                                {
                                    if (int.Parse(trailsQueue[i][j, 1]) * 2 + 1 + moveX < arrayWidth * 2 && int.Parse(trailsQueue[i][j, 2]) + moveY < arrayHeight)
                                    {
                                        Console.CursorLeft = int.Parse(trailsQueue[i][j, 1]) * 2 + 1 + moveX;
                                        Console.CursorTop = int.Parse(trailsQueue[i][j, 2]) + 1 + moveY;
                                        Console.Write("  ");
                                    }
                                }
                                trailsQueue[i][j, 1] = posX.ToString();
                                trailsQueue[i][j, 2] = posY.ToString();
                                trailsQueue[i][j, 3] = trailLenght.ToString();

                                break;
                            }
                        }
                    }
                    break;
                }
            }
        }

        static int[,] XYPosToArray(List<Body> Objects, ref double distanceScale, int arrayHeight, int arrayWidth, int[,] xyPos)
        {
            double tablePosX;
            double tablePosY;

            int Adjust;
            Adjust = (arrayWidth - 1) / 2 - (arrayHeight - 1) / 2;

            for (int i = 0; i < Objects.Count; i++)
            {
                tablePosX = (arrayHeight - 1) / 2 + (Objects[i].posX / distanceScale) * (arrayHeight - 1) / 2 + Adjust;

                tablePosY = (arrayHeight - 1) / 2 - (Objects[i].posY / distanceScale) * (arrayHeight - 1) / 2;

                xyPos[i, 0] = (int)Math.Round(tablePosX);
                xyPos[i, 1] = (int)Math.Round(tablePosY);


            }
            return xyPos;
        }

        static void trailPrinter(List<string[,]> trailsQueue, int trailLenght, int moveX, int moveY, int arrayHeight, int arrayWidth)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = 0; i < trailsQueue.Count; i++)
            {
                for (int j = 0; j < trailLenght; j++)
                {
                    if (trailsQueue[i][j, 3] != null)
                    {
                        if (int.Parse(trailsQueue[i][j, 1]) * 2 + 1 + moveX < arrayWidth * 2 && int.Parse(trailsQueue[i][j, 2]) + moveY < arrayHeight)
                        {
                            if (int.Parse(trailsQueue[i][j, 1]) * 2 + 1 + moveX > 0 && int.Parse(trailsQueue[i][j, 2]) + 1 + moveY > 0)
                            {
                                Console.CursorLeft = int.Parse(trailsQueue[i][j, 1]) * 2 + 1 + moveX;
                                Console.CursorTop = int.Parse(trailsQueue[i][j, 2]) + 1 + moveY;
                                Console.Write(trailsQueue[i][j, 0].Substring(0, 2));
                            }
                        }
                        trailsQueue[i][j, 3] = (int.Parse(trailsQueue[i][j, 3]) - 1).ToString();
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.CursorTop = arrayHeight + 2;
            Console.CursorLeft = 0;
        }

        static void centreView(int[,] xyPos, int centredPlanet, ref int moveX, ref int moveY, int arrayHeight, int arrayWidth)
        {
            int centreX = (int)Math.Round((double)(arrayWidth) / 2);
            int centreY = (int)Math.Round((double)(arrayHeight) / 2);

            int xCentreDiff = centreX - xyPos[centredPlanet, 0];
            int yCentreDiff = centreY - xyPos[centredPlanet, 1];

            moveX += xCentreDiff * 2;
            moveY += yCentreDiff;
        }

        static int[,] altcentreView(List<Body> Objects, int[,] xyPos, int centredPlanet, int moveX, int moveY, int arrayHeight, int arrayWidth, ref List<string[,]> trailsQueue, int trailLenght)
        {
            int centreX = (int)Math.Round((double)(arrayWidth) / 2);
            int centreY = (int)Math.Round((double)(arrayHeight) / 2);

            int xCentreDiff = centreX - xyPos[centredPlanet, 0];
            int yCentreDiff = centreY - xyPos[centredPlanet, 1];

            for (int i = 0; i < Objects.Count; i++)
            {
                xyPos[i, 0] = xyPos[i, 0] + xCentreDiff;
                xyPos[i, 1] = xyPos[i, 1] + yCentreDiff;
                cycleTrailQueue(xyPos[i, 0], xyPos[i, 1], ref trailsQueue, Objects[i].name, trailLenght, moveX, moveY, arrayHeight, arrayWidth);
            }
            return xyPos;
        }

        static int[,] removeOverlap(List<Body> Objects, int[,] xyPos)
        {
            int[,] newXY = new int[Objects.Count, 3];
            for (int i = 0; i < Objects.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    newXY[i, j + 1] = xyPos[i, j];
                }
                newXY[i, 0] = i;
            }

            for (int i = 0; i < Objects.Count - 1; i++)
            {
                for (int j = i + 1; j < Objects.Count; j++)
                {
                    if (newXY[i, 0] != -1 && newXY[j, 0] != -1)
                    {
                        if (newXY[i, 1] == newXY[j, 1] && newXY[i, 2] == newXY[j, 2])
                        {
                            if (Objects[newXY[i, 0]].mass > Objects[newXY[j, 0]].mass)
                            {
                                newXY[j, 0] = -1;
                            }
                            else
                            {
                                newXY[i, 0] = -1;
                            }
                        }
                    }
                }
            }

            return newXY;
        }

        static void visualPosition(List<Body> Objects, ref double largestDistance, ref List<string[,]> trailsQueue, int trailLenght, int centredPlanet, int centredType, int moveX, int moveY, bool useTrails, int arrayHeight, int arrayWidth)
        {

            int[,] xyPos = new int[Objects.Count, 2];
            string[,] table = new string[arrayHeight, arrayWidth];

            XYPosToArray(Objects, ref largestDistance, arrayHeight, arrayWidth, xyPos);

            if (Objects.Count == 2)
            {
                boxCleaner(arrayHeight, arrayWidth);
            }

            if (centredType == 0)
            {
                for (int i = 0; i < Objects.Count; i++)
                {
                    cycleTrailQueue(xyPos[i, 0], xyPos[i, 1], ref trailsQueue, Objects[i].name, trailLenght, moveX, moveY, arrayHeight, arrayWidth);
                }
            }

            if (centredPlanet > -1)
            {
                if (centredType == 1)
                {
                    centreView(xyPos, centredPlanet, ref moveX, ref moveY, arrayHeight, arrayWidth);
                    for (int i = 0; i < Objects.Count; i++)
                    {
                        cycleTrailQueue(xyPos[i, 0], xyPos[i, 1], ref trailsQueue, Objects[i].name, trailLenght, moveX, moveY, arrayHeight, arrayWidth);
                    }
                    boxCleaner(arrayHeight, arrayWidth);
                }
                else if (centredType == 2)
                {
                    xyPos = altcentreView(Objects, xyPos, centredPlanet, moveX, moveY, arrayHeight, arrayWidth, ref trailsQueue, trailLenght);
                }
            }

            if (useTrails)
            {
                trailPrinter(trailsQueue, trailLenght, moveX, moveY, arrayHeight, arrayWidth);
            }

            xyPos = removeOverlap(Objects, xyPos);

            cursorArrayPrinter(Objects, xyPos, moveX, moveY, arrayHeight, arrayWidth);
        }
    }
}