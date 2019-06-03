using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class that returns a random value from a collection without repeating the value.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Randomizer<T>
{
    private readonly List<int> _indexesToGenerate;
    private readonly IReadOnlyList<T> _values;

    public Randomizer(IReadOnlyList<T> values)
    {
        _values = values;
        _indexesToGenerate = Enumerable.Range(0, _values.Count()).ToList();
    }

    public T GetNext()
    {
        if (_indexesToGenerate.Count == 0)
        {
            _indexesToGenerate.AddRange(Enumerable.Range(0, _values.Count()).ToList());
        }

        int randIndex = Random.Range(0, _indexesToGenerate.Count);
        var randValue = _indexesToGenerate[randIndex];
        _indexesToGenerate.Remove(randValue);

        return _values[randValue];
    }
}