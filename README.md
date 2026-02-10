## Last resuls:
```
Benchmark concluded:
Benchmark                       | Best (Avr.)(μs) | Worst (Avr.)(μs) | Complexity
------------------------------- | --------------- | ---------------- | ----------
Eclipse IService.Instance       | 0.0117          | 0.0103           | O(1)      
Naninovel Engine.GetService<>() | 0.0345          | 0.0345           | O(1)      
RimWorld Game.GetComponent<>()  | 0.0167          | 0.4001           | O(n) n:7  
Native GetField                 | 0.0104          | 0.0101           | O(1)      
Idle (Control)                  | 0.0003          | 0.0003           | O(1)      
```
*Note: "Best (Avr.)" is larger than "Worst (Avr.)" because we do two separate benchmarks for best and worst cases.*
*It's made this way so we can change a `Type` in `Game.GetComponent<T>()` and similar methods.*
