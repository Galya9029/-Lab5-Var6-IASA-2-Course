using System;

namespace LabVar6
{
    enum Periodicity
    {
        Weekly,
        Monthly,
        Quarterly
    }

    class Author
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthday { get; set; }

        public Author(string name, string surname, DateTime birthday)
        {
            Name = name;
            Surname = surname;
            Birthday = birthday;
        }

        public Author() : this("", "", new DateTime()){ }
        public Author(Author author) : this(author.Name, author.Surname, author.Birthday){ }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Author m=obj as Author;
            if (m == null) return false;
            else return (Name == m.Name && Surname == m.Surname && Birthday == m.Birthday);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + Surname.GetHashCode() + Birthday.GetHashCode();
        }

        public static bool operator ==(Author obj1, Author obj2)
        {
            return (obj1.Equals(obj2));
        }

        public static bool operator !=(Author obj1, Author obj2)
        {
            return !(obj1.Equals(obj2));
        }

        public override string ToString()
        {
            return string.Format("Name: {0}\nSurname: {1}\nBirthday: {2}", 
                Name, Surname, Birthday.ToString());
        }

        public object Clone()
        {
            return new Author(this);
        }

        public string PartPrint()
        {
            return string.Format("\nName: {0}\nSurname: {1}\n", Name, Surname);
        }
    }

    class Article : IComparable
    {
        public  Author ThisAuthor { get; set; }
        public string Title { get; set; }
        public int Pages { get; set; } 
        public int Cost { get; set; }

        public Article(Author this_author, string title, int pages, int cost)
        {
            ThisAuthor = this_author;
            Title = title;
            Pages = pages;
            Cost = cost;
        }
        
        public Article(): this(new Author(), "", 0, 0 ){ }
        public Article(Article article): this(article.ThisAuthor, article.Title, article.Pages, article.Cost){ }
       
        public int CompareTo(object obj)
        {
            Article temp = obj as Article;
            if (temp != null)
            {
                if (this.Pages > temp.Pages) return 1;
                if (this.Pages < temp.Pages) return -1;
                else return 0;
            }
            else throw new ArgumentException("Parameter is not an Atricle");
        }

        public static bool operator<(Article obj1, Article obj2)
        {
            return (obj1.CompareTo(obj2) == -1);
        }
        
        public static bool operator>(Article obj1, Article obj2)
        {
            return (obj1.CompareTo(obj2) == 1);
        }

        public override string ToString()
        {
            return string.Format("Author: {0}\nTitle: {1}\nAmount of pages: {2}\nCost: {3}",
                ThisAuthor.ToString(), Title, Pages.ToString(), Cost.ToString());
        }

        public object Clone()
        {
            return new Article(this);
        }

        public string PartPrint()
        {
            return string.Format("\nAmount of pages: {0}\n", Pages);
        }
    }        
    class Magazine : ICloneable
    {
        public Periodicity Period {get; set;}
        public Author THisAuthor { get; set; }
        public string MagazineTitle { get; set; }
        public int Size { get; set; }
        public Article[] ArticlesInfo;

        public Magazine(Periodicity period, Author thisAuthor, string magazineTitle, int size, Article[] articles)
        { 
            Period = period;
            THisAuthor = thisAuthor;
            MagazineTitle = magazineTitle;
            Size = size;
            Article[] temp = new Article[articles.Length];
            for (int i = 0; i < articles.Length; i++)
            {
                temp[i] = articles[i];
            }
            ArticlesInfo = temp;
        }
        
        public Magazine(): this(Periodicity.Weekly, new Author(), "", 0, new Article[1]){ }
        public Magazine(Magazine magazine): this(magazine.Period, magazine.THisAuthor, magazine.MagazineTitle, magazine.Size, magazine.ArticlesInfo){ }

        public object Clone()
        {
            Article[] temp = new Article[ArticlesInfo.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = ArticlesInfo[i];
            }
            return new Magazine(this.Period, this.THisAuthor, this.MagazineTitle, this.Size, temp);
        }

        public void AddArticle(Article article)
        {
            Array.Resize(ref this.ArticlesInfo, ArticlesInfo.Length + 1);
            ArticlesInfo[ArticlesInfo.Length - 1] = article;
        }

        public Article this[int index]
        {
            get { return ArticlesInfo[index]; }
            set { ArticlesInfo[index] = value; }
        }
            
        public string PartPrint()
        {
            return string.Format("\nMagazine's Name: {0}\nAmount of Articles: {1}\n", MagazineTitle, Size);
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Example for \"Author\" class:");
            DateTime date1 = new DateTime(1828, 9, 9);
            Author author1 = new Author("Lev", "Tolstoy",date1);
            Console.WriteLine(author1);
            Author author2 = author1.Clone() as Author;
            author2.Name = "King";
            author2.Surname = "Stephen";
            author2.Birthday = new DateTime(1947, 9, 21);
            Console.WriteLine(author2);
            Console.WriteLine("Are the authors equal: {0}", author1 == author2);
            Console.WriteLine("Example of partial print: {0}", author2.PartPrint());
            
            Console.WriteLine("Example for \"Article\" class:");
            Article article1 = new Article(author1, "Anna Karenina", 864, 81);
            Article article2 = article1.Clone() as Article;
            article2.Pages = 1270;
            article2.Cost = 150;
            article2.Title = "War and Peace";
            Console.WriteLine("Example of ToString method of Article:\n{0}", article2);
            Console.WriteLine("\nExample of result of comparison operator < : {0}", article1<article2);
            Console.WriteLine("Example of partial print: {0}", article2.PartPrint());
            Console.WriteLine("Example for \"Magazine\" class");
            Article[] articles = {article1, article2};
            Magazine magazine1 = new Magazine(Periodicity.Monthly, author1, "Literature", 100, articles);
            Magazine magazine2 = magazine1.Clone() as Magazine;
            Magazine magazine3 = new Magazine();
            magazine3.Period = Periodicity.Quarterly;
            magazine3.Size = 10;
            magazine3.MagazineTitle = "Literature";
            magazine3.AddArticle(article2.Clone() as Article);
            Console.WriteLine(magazine3.PartPrint());
        }
    }
}