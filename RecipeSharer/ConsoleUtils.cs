namespace Utils;
public class ConsoleUtils
{
    public int GetValidRatingFromUser()
{
    int rating;
    do
    {
        Console.Write("Please enter a rating from 0 to 10: ");
        string input = Console.ReadLine();
        if (int.TryParse(input, out rating) && rating >= 0 && rating <= 10)
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid rating. Please enter a number from 1 to 10.");
        }
    } while (true);

    return rating;
}

}