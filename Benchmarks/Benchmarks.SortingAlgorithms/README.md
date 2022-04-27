### 1000 element int[]
|                           Method |         Mean |       Error |      StdDev |       Median |   Gen 0 | Allocated |
|--------------------------------- |-------------:|------------:|------------:|-------------:|--------:|----------:|
|                       BubbleSort | 2,716.121 us | 128.0151 us | 367.2994 us | 2,653.492 us |       - |       2 B |
|               BubbleSortImproved |     2.091 us |   0.0793 us |   0.2302 us |     2.025 us |       - |         - |
|                    SelectionSort |   867.443 us |  61.8881 us | 180.5305 us |   807.088 us |       - |       1 B |
| SelectionSortSwapOnlyOncePerLoop |   963.487 us |  37.6213 us | 109.1462 us |   931.731 us |       - |         - |
|                        MergeSort |   266.248 us |  16.5559 us |  48.5555 us |   260.621 us | 60.0586 | 252,961 B |
This BubbleSortImproved time can not be right. I've checked the actual algo results multiple times though and it is sorting things fine though.

Need to revisit later.