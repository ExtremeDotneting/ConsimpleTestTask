using AutoMapper;

namespace ConsimpleTestTask.WebApp.Utils
{
    public static class GlobalMapper
    {
        static IMapper _mapper;

        public static void Init(IMapper mapper)
        {
            _mapper = mapper;
            
        }

        public static TDestination Map<TDestination, TSource>(TSource source)
            where TDestination : class
        {
            return _mapper.Map<TDestination>(source);
        }

        public static void Map<TDestination, TSource>(TSource source, TDestination destination)
            where TDestination : class
            where TSource : class
        {

            _mapper.Map(source, destination);
        }
    }
}
