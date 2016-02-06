namespace Padstone
{
    public interface IRangeExtremumQuery<T>
	{
		bool TryGetMinimum(int lowerInclusiveBound, int upperInclusiveBound, out T result);

		bool TryGetMaximum(int lowerInclusiveBound, int upperInclusiveBound, out T result);
	}
}
