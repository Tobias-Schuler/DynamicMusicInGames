using System;
using System.Diagnostics;
using UnityEngine;

public class PerformanceTest : MonoBehaviour {

    int cycles = 10; //setting this value too high will take a considerable time (100 took > 1min when tried)

    void Start () {
        FunctionComparison f = new FunctionComparison(10_000_000);
        long mathPowTotalTime = 0;
        long multiplyThriceTotalTime = 0;
        for(int i = 0; i < cycles; i++) {
            mathPowTotalTime += f.MathPow();
            multiplyThriceTotalTime += f.MultiplyThrice();
        }
        UnityEngine.Debug.Log("Math.Pow() average time after " + cycles + " runs: " + (mathPowTotalTime / cycles));
        UnityEngine.Debug.Log("Multiply Thrice average time after " + cycles + " runs: " + (multiplyThriceTotalTime / cycles));
    }

    // Update is called once per frame
    void Update () {

    }
}

//class to test 2 functions and return the time it took to compute them
class FunctionComparison {

    int cycles = 0;

    public FunctionComparison (int cycles) {
        this.cycles = cycles;
    }

    public long MultiplyThrice () {
        Stopwatch s = new Stopwatch();
        s.Start();
        for(int i = 0; i < cycles; i++) {
            double nonInteger = i * 0.5d;
            double test = nonInteger * nonInteger * nonInteger;
        }
        s.Stop();
        return s.ElapsedMilliseconds;
    }

    public long MathPow () {
        Stopwatch s = new Stopwatch();
        s.Start();
        for(int i = 0; i < cycles; i++) {
            double nonInteger = i * 0.5d;
            double test = Math.Pow(i, 3);
        }
        s.Stop();
        return s.ElapsedMilliseconds;
    }

}
