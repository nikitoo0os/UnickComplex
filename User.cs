using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ComplexProject;

public partial class User
{
    public int IdUser { get; set; }

    public int IdWallet { get; set; }

    public long IdSupportProject { get; set; }

    public long IdProject { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string SecondName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public DateOnly DateOfRegistration { get; set; }

    public string Role { get; set; } = "user";
    public string? PathImage { get; set; }

    public virtual ICollection<Activity> Activities { get; } = new List<Activity>();

    public virtual ICollection<Auctionlot> Auctionlots { get; } = new List<Auctionlot>();

    public virtual ICollection<Bid> Bids { get; } = new List<Bid>();

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual ICollection<Conversation> ConversationIdUser1Navigations { get; } = new List<Conversation>();

    public virtual ICollection<Conversation> ConversationIdUserNavigations { get; } = new List<Conversation>();

    public virtual ICollection<Donation> Donations { get; } = new List<Donation>();

    public virtual ICollection<Favouritelot> Favouritelots { get; } = new List<Favouritelot>();

    public virtual ICollection<Favouritetag> Favouritetags { get; } = new List<Favouritetag>();

    public virtual ICollection<Project> Projects { get; } = new List<Project>();

    public virtual ICollection<Wallet> Wallets { get; } = new List<Wallet>();
}
