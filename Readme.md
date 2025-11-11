# Açıklama
Sitede online olan kullanıcıları göstermek için kullanılan bir bileşen olan ZoeCounter'ın belgeleri.

## ZoeCounter Bileşeni
```CSharp
@page "/online-users"
@inject ZoeUserCounter.WhoIsOnline WhoIsOnline
@inject ZoeUserCounter.UserTracker Tracker

<h3>Aktif kullanıcı sayısı: @count</h3>

@if (users.Count == 0)
{
    <p>Şu anda çevrimiçi kullanıcı bulunmuyor.</p>
}
else
{
    <h3 class="mb-3 text-primary">Online Kullanıcılar (@users.Count)</h3>
    <ul class="list-group">
        @foreach (var user in users)
        {
            <li class="list-group-item">@user</li>
        }
    </ul>
}

@code {
    private int count;
    private List<string> users = new();

    protected override void OnInitialized()
    {
        count = Tracker.OnlineCount;
        Tracker.OnCountChanged += UpdateCount;

        // Başlangıçta mevcut kullanıcıları al
        users = WhoIsOnline.OnlineUsers.ToList();

        // Gerçek zamanlı değişiklikleri dinle
        WhoIsOnline.OnUsersChanged += RefreshUsers;
    }

    private void RefreshUsers()
    {
        // Arka planda gelen event UI thread'ine taşınıyor
        InvokeAsync(() =>
        {
            users = WhoIsOnline.OnlineUsers.ToList();
            StateHasChanged();
        });
    }

    public void Dispose()
    {
        // Bellek sızıntısını önlemek için event'ten çık
        WhoIsOnline.OnUsersChanged -= RefreshUsers;

        Tracker.OnCountChanged -= UpdateCount;
    }

    private void UpdateCount()
    {
        InvokeAsync(() =>
        {
            count = Tracker.OnlineCount;
            StateHasChanged();
        });
    }
}

```
## Özellikler
- **UserTracker**: buradan online sayısı gelir. Burada her sayfa için ekleme olur kişi distinct değil.
- **WhoIsOnlie**: Burada kişi distinct olur login olmayan ise hepsi için Anonim olur.
- 
## Uyarı
>! AuthenticationStateProvider kullanıyor User.Identity.Name bilgisiyle User alınıyor.
