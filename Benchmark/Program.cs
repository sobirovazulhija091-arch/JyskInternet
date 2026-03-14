using BenchmarkDotNet.Running;
using Benchmark.Benchmarks;

BenchmarkRunner.Run<GetProducts>();
BenchmarkRunner.Run<GetBrands>();
