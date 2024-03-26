// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("#searchInp").on("keyup", async function () {
    
    let searchInpValue = $(this).val();
    let url = `https://localhost:44309/Employee/Index?searchInp=${searchInpValue}`;

    //using fetch
    let Response = await fetch(url,
        {
            Method: 'POST',
        });
    let text = await Response.text();
    console.log(text);

    //let xhr = new XMLHttpRequest();
    //xhr.open("Get", url);
    //xhr.onreadystatechange = function () {
    //    if (this.readyState == 4 && this.status == 200)
    //    {
    //        console.log(this.responseText);
    //    }
    //}
    //xhr.send();
})