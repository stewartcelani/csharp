100 runs of downloading 1000 randomly generated files between 0 and 5mb in size.

Parallel loop 45% faster in this benchmark.

|          Method |     Mean |    Error |   StdDev |     Gen 0 |     Gen 1 | Allocated |
|---------------- |---------:|---------:|---------:|----------:|----------:|----------:|
| SynchronousLoop | 11.472 s | 0.3365 s | 0.9654 s | 2000.0000 | 1000.0000 |      8 MB |
|    ParallelLoop |  6.355 s | 0.2787 s | 0.8087 s | 2000.0000 | 1000.0000 |      8 MB |
