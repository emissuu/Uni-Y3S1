namespace Lab10;

public class KataCalculator
{
    public int IntAdd(string input)
    {
        var result = Add(input);

        if (!int.TryParse(result, out int resultInt))
        {
            throw new FormatException(result);
        }

        return resultInt;
    }
    
    public string Add(string input)
    {
        var (values, errorMessages) = Parse(input);
        
        if (errorMessages.Count == 0)
        {
            return values.Sum().ToString();
        }
        return String.Join('\n', errorMessages);
    }

    public string Multiply(string input)
    {
        var (values, errorMessages) = Parse(input);

        if (errorMessages.Count == 0)
        {
            var product = 1;

            foreach (var value in values)   
                product *= value;
            return product.ToString();
        }
        return String.Join('\n', errorMessages);
    }

    private (List<int>, List<string>) Parse(string input)
    {
        if (input.IsWhiteSpace())
            return ([0], []);
        
        List<string> delimiters = ["\n", ","];

        if (input.StartsWith("//"))
        {
            var delimiterEnd = input.IndexOf('\n');
            var delimiter =  input[2..delimiterEnd];
            delimiters[1] = delimiter;
            input = input[(delimiterEnd + 1)..];
        }
        
        int position = -1;
        int lastDelimiter = -1;
        List<int> values = [];
        List<int> negativeValues = [];
        List<string> errorMessages = [];
        
        while (position < input.Length)
        {
            position++;
            
            if (position == input.Length || input[position].ToString() == delimiters[0] || input[position..].StartsWith(delimiters[1]))
            {
                if (position == input.Length - 1)
                {
                    errorMessages.Add("Number expected but EOF found.");
                    break;
                }
                
                if (lastDelimiter + 1 == position)
                {
                    errorMessages.Add($"Number expected but '{input[position]}' found at position {position}.");
                    lastDelimiter = position;
                    continue;
                }
                
                var delimiterLength = 1;
                if (input[position..].StartsWith(delimiters[1]))
                {
                    delimiterLength = delimiters[1].Length;
                }
                var value = int.Parse(input.Substring(lastDelimiter + 1, position - lastDelimiter - 1));
                
                if (value < 0) negativeValues.Add(value);
                else values.Add(value);
                
                lastDelimiter = position + delimiterLength - 1;
                
                if (input[position..].StartsWith(delimiters[1]))
                    position += delimiters[1].Length - 1;
                continue;
            }
            
            if(int.TryParse(input[position].ToString(), out int _) || input[position] == '-')
               continue;

            errorMessages.Add($"\'{delimiters[1]}\' expected but \'{input[position]}\' found at position {position}.");
            lastDelimiter = position;
        }

        if (negativeValues.Count != 0)
            errorMessages.Add($"Negative not allowed: {String.Join(", ", negativeValues)}");
        
        return (values, errorMessages);
    }
}