namespace Heus.Ddd.Application.Services
{

    public interface ICrudAppService<TEntityDto, in TKey, in TGetListInput>
        : ICrudAppService<TEntityDto, TKey, TGetListInput, TEntityDto>
    {

    }

    public interface ICrudAppService<TEntityDto, in TKey, in TGetListInput, in TCreateInput>
        : ICrudAppService<TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>
    {

    }

    public interface ICrudAppService<TEntityDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
        : ICrudAppService<TEntityDto, TKey, TGetListInput, TEntityDto, TCreateInput, TUpdateInput>
    {

    }

    public interface ICrudAppService<TGetOutputDto, in TKey, in TGetListInput, TGetListOutputDto, in TCreateInput,
            in TUpdateInput>
        : IReadOnlyAppService<TGetOutputDto, TKey, TGetListInput, TGetListOutputDto>,
            ICreateAppService<TGetOutputDto, TCreateInput>,
            IUpdateAppService<TGetOutputDto, TUpdateInput>,
            IDeleteAppService<TKey>
    {

    }
}