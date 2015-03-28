using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KomiwojazerGoogleMaps.Classes;

namespace KomiwojazerGoogleMaps.Algorithm
{
    public static class TravelingSalesmanProblemSolver
    {
        public enum Result : byte
        {
            Success,
            GoogleConnectionError,
            HamiltonianCycleNotFound
        };

        private static Result mResult;
        private static int mNumberOfBts;
        private static int mStartVertex;
        private static double mOptimalRouteDistance;
        private static double mWeightsSum;
        private static int mRouteIndex;
        private static int mRouteTemporaryIndex;

        private static double[,] mWeights;
        private static bool[,] mNeighborhoodMatrix;
        private static int[] mRoute;
        private static int[] mRouteTemporary;
        private static bool[] mVisited;

        private static List<Database.Bt> mBtsList;

        public static Result solve(List<Database.Bt> btsList)
        {
            mBtsList = btsList;
            mResult = Result.Success;

            initializeInternalData();
            if (!createRoutesBetweenAllAvailablesLocations())
            {
                return mResult;
            }

            runAlgorithm(mStartVertex);

            if (mRouteIndex == 0)
            {
                mResult = Result.HamiltonianCycleNotFound;
            }

            return mResult;
        }

        public static List<int> getOptimalRoute()
        {
            List<int> result = new List<int>();
            for (int i = 0; i < mRouteIndex; ++i)
            {
                result.Add(mRoute[i]);
            }
            result.Add(mStartVertex);
            return result;
        }

        public static double getOptimalRouteDistance()
        {
            return mOptimalRouteDistance;
        }

        private static void initializeInternalData()
        {
            mNumberOfBts = mBtsList.Count;
            mRoute = new int [mNumberOfBts];
            mRouteTemporary = new int [mNumberOfBts];
            mRouteIndex = 0;
            mRouteTemporaryIndex = 0;
            mVisited = new bool [mNumberOfBts];
            mWeights = new double [mNumberOfBts, mNumberOfBts];
            mNeighborhoodMatrix = new bool [mNumberOfBts, mNumberOfBts];
            mOptimalRouteDistance = int.MaxValue;
            mWeightsSum = 0;
            mStartVertex = 0;

            for (int i = 0; i < mNumberOfBts; i++)
            {
                for (int j = 0; j < mNumberOfBts; j++)
                {
                    mNeighborhoodMatrix[i, j] = false;
                    mWeights[i, j] = 0;
                }

                mVisited[i] = false;
            }
        }

        private static bool createRoutesBetweenAllAvailablesLocations()
        {
            for (int i = 0; i < mNumberOfBts; i++)
            {
                for (int j = 0; j < mNumberOfBts; j++)
                {
                    if (i != j)
                    {
                        mNeighborhoodMatrix[i, j] = mNeighborhoodMatrix[j, i] = true;
                        try
                        {
                            mWeights[i, j] = mWeights[j, i] = BtsFinder.getDistanceBetweenLocations(mBtsList[i], mBtsList[j]);
                        }
                        catch
                        {
                            mResult = Result.GoogleConnectionError;
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private static void runAlgorithm(int vertex)
        {
            int index = 0;

            mRouteTemporary[mRouteTemporaryIndex++] = vertex;

            if (mRouteTemporaryIndex < mNumberOfBts)
            {
                mVisited[vertex] = true;
                for (index = 0; index < mNumberOfBts; ++index)
                {
                    if (mNeighborhoodMatrix[vertex, index] && !mVisited[index])
                    {
                        mWeightsSum += mWeights[vertex, index];
                        runAlgorithm(index);
                        mWeightsSum -= mWeights[vertex, index];
                    }
                }
                mVisited[vertex] = false;
            }
            else if (mNeighborhoodMatrix[mStartVertex, vertex])
            {
                mWeightsSum += mWeights[vertex, mStartVertex];
                if (mWeightsSum < mOptimalRouteDistance)
                {
                    mOptimalRouteDistance = mWeightsSum;
                    for (index = 0; index < mRouteTemporaryIndex; ++index)
                    {
                        mRoute[index] = mRouteTemporary[index];
                    }
                    mRouteIndex = mRouteTemporaryIndex;
                }
                mWeightsSum -= mWeights[vertex, mStartVertex];
            }
            --mRouteTemporaryIndex;
        }
    }
}
