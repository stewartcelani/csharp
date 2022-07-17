namespace GenericMethodsAndDelegates.Entities;

/*
 * This was not part of the course
 */
public interface IAuditable
{
    DateTime CreatedDate { get; set; }
    DateTime? ModifiedDate { get; set; }
}