namespace App.validate
{
    public interface IValidator<T>
    {
        List<T> Errors { get; set; }
   
    }
}
