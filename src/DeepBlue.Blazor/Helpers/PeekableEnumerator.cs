
using System.Collections;
using System.ComponentModel;

namespace DeepBlue.Blazor.Helpers;

public class PeekableEnumerator<T> : IEnumerator<T>
{
  private IEnumerator _enumeraotr;
  private T? _peek;
  private bool _didPeek;

  public T Current => _didPeek && _peek is not null ? _peek : (T)_enumeraotr.Current;

  object IEnumerator.Current
  {
    get => this.Current ?? default(T);
  }

  public PeekableEnumerator(IEnumerator enumerator)
  {
    if (enumerator == null)
      throw new ArgumentNullException(nameof(enumerator), "Enumerator is null");

    _peek = default(T);
    _enumeraotr = enumerator;
  }

  public bool MoveNext()
  {
    return _didPeek ? !(_didPeek = false) : _enumeraotr.MoveNext();
  }

  public void Reset()
  {
    _enumeraotr.Reset();
    _didPeek = false;
  }

  public void Dispose()
  {
    throw new NotImplementedException();
  }

  public bool TryPeek(out T? result)
  {
    if (!_didPeek && (_didPeek = _enumeraotr.MoveNext()))
    {
      result = (T)_enumeraotr.Current;
      _peek = result;
      return true;
    }
    result = default(T);
    return false;
  }

}
