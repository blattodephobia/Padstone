namespace Padstone
{
    public interface IRangeExtremumQuery<T>
	{
		T FindMinimum(int lowerInclusiveBound, int upperInclusiveBound);

		T FindMaximum(int lowerInclusiveBound, int upperInclusiveBound);
	}
}
