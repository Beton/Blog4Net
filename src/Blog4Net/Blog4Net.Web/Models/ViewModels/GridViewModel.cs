namespace Blog4Net.Web.Models.ViewModels
{
    public class GridViewModel
    {
        // no. of records to fetch
        public int Rows
        { get; set; }

        // the page index
        public int Page
        { get; set; }

        // sort column name
        public string Sidx
        { get; set; }

        // sort order "asc" or "desc"
        public string sord
        { get; set; }
    }
}