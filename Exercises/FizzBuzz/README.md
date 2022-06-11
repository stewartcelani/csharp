|                                          Method |     Mean |    Error |    StdDev |   Median | Allocated |
|------------------------------------------------ |---------:|---------:|----------:|---------:|----------:|
|                     StringBuilderImplementation | 64.11 ms | 6.847 ms | 20.188 ms | 62.39 ms |     13 KB |
| StringBuilderWithDeclaredCapacityImplementation | 29.68 ms | 6.923 ms | 19.412 ms | 19.45 ms |     12 KB |
|                            StringImplementation | 28.53 ms | 5.613 ms | 15.924 ms | 19.59 ms |      2 KB |
|                      StringElseIfImplementation | 18.78 ms | 0.374 ms |  0.971 ms | 18.84 ms |      2 KB |

Surpsied my initial StringBuilderImplementation is slower and uses more memory than string implementations. Isn't using
StringBuilder meant to use less memory than manipulating (immutable) strings?

Makes sense that the StringElseIfImplementation is the fastest as it short-circuts subsequent evaluations with the else
if statements at the cost of looking less 'clean' (to me).