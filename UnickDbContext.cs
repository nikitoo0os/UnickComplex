using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ComplexProject;

public partial class UnickDbContext : DbContext
{
    public UnickDbContext()
    {
    }

    public UnickDbContext(DbContextOptions<UnickDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Auctionlot> Auctionlots { get; set; }

    public virtual DbSet<Auctiontag> Auctiontags { get; set; }

    public virtual DbSet<Bid> Bids { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<Crowdfundingtag> Crowdfundingtags { get; set; }

    public virtual DbSet<Donation> Donations { get; set; }

    public virtual DbSet<Favouritelot> Favouritelots { get; set; }

    public virtual DbSet<Favouritetag> Favouritetags { get; set; }
    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=unick_db;Username=postgres;Password=@8acJMQKc");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.IdActivity).HasName("activities_pkey");

            entity.ToTable("activities", "unick_schema");

            entity.Property(e => e.IdActivity).HasColumnName("id_activity");
            entity.Property(e => e.IdLot).HasColumnName("id_lot");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.Time).HasColumnName("time");

            entity.HasOne(d => d.IdLotNavigation).WithMany(p => p.Activities)
                .HasForeignKey(d => d.IdLot)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("activities_id_lot_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Activities)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("activities_id_user_fkey");
        });

        modelBuilder.Entity<Auctionlot>(entity =>
        {
            entity.HasKey(e => e.IdLot).HasName("auctionlots_pkey");

            entity.ToTable("auctionlots", "unick_schema");

            entity.Property(e => e.IdLot).HasColumnName("id_lot");
            entity.Property(e => e.AttachmentsLink).HasColumnName("attachments_link");
            entity.Property(e => e.AverageProfit)
                .HasColumnType("money")
                .HasColumnName("average_profit");
            entity.Property(e => e.Category)
                .HasMaxLength(15)
                .HasColumnName("category");
            entity.Property(e => e.EndPrice)
                .HasColumnType("money")
                .HasColumnName("end_price");
            entity.Property(e => e.IdAuctioneer).HasColumnName("id_auctioneer");
            entity.Property(e => e.ImageLink).HasColumnName("image_link");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasColumnName("location");
            entity.Property(e => e.PaybackTime).HasColumnName("payback_time");
            entity.Property(e => e.StartPrice)
                .HasColumnType("money")
                .HasColumnName("start_price");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .HasColumnName("status");
            entity.Property(e => e.Type)
                .HasMaxLength(11)
                .HasColumnName("type");
            entity.Property(e => e.ViewerCount).HasColumnName("viewer_count");
            entity.Property(e => e.Winner)
                .HasMaxLength(15)
                .HasColumnName("winner");

            entity.Property(e => e.Title)
            .HasMaxLength(100)
            .HasColumnName("title");

            entity.Property(e => e.Description)
            .HasMaxLength(1000)
            .HasColumnName("description");

            entity.Property(e => e.StartDate)
            .HasColumnName("start_date");

            entity.Property(e => e.CurrentPrice)
            .HasColumnName("current_price");

            entity.Property(e => e.EndDate)
            .HasColumnName("end_date");


            entity.HasOne(d => d.IdAuctioneerNavigation).WithMany(p => p.Auctionlots)
                .HasForeignKey(d => d.IdAuctioneer)
                .HasConstraintName("auctionlots_id_auctioneer_fkey");
        });

        modelBuilder.Entity<Auctiontag>(entity =>
        {
            entity.HasKey(e => e.IdAuctionTag).HasName("auctiontags_pkey");

            entity.ToTable("auctiontags", "unick_schema");

            entity.Property(e => e.IdAuctionTag).HasColumnName("id_auction_tag");
            entity.Property(e => e.IdLot).HasColumnName("id_lot");
            entity.Property(e => e.IdTag).HasColumnName("id_tag");

            entity.HasOne(d => d.IdLotNavigation).WithMany(p => p.Auctiontags)
                .HasForeignKey(d => d.IdLot)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("auctiontags_id_lot_fkey");

            entity.HasOne(d => d.IdTagNavigation).WithMany(p => p.Auctiontags)
                .HasForeignKey(d => d.IdTag)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("auctiontags_id_tag_fkey");
        });

        modelBuilder.Entity<Bid>(entity =>
        {
            entity.HasKey(e => e.IdBid).HasName("bids_pkey");

            entity.ToTable("bids", "unick_schema");

            entity.Property(e => e.IdBid).HasColumnName("id_bid");
            entity.Property(e => e.IdLot).HasColumnName("id_lot");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.Time).HasColumnName("time");

            entity.HasOne(d => d.IdLotNavigation).WithMany(p => p.Bids)
                .HasForeignKey(d => d.IdLot)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bids_id_lot_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Bids)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bids_id_user_fkey");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.IdComment).HasName("comments_pkey");

            entity.ToTable("comments", "unick_schema");

            entity.Property(e => e.IdComment).HasColumnName("id_comment");
            entity.Property(e => e.IdProject)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_project");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Text)
                .HasMaxLength(256)
                .HasColumnName("text");
            entity.Property(e => e.Timestamp).HasColumnName("timestamp");

            entity.HasOne(d => d.IdProjectNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.IdProject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comments_id_project_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comments_id_user_fkey");
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.IdConversation).HasName("conversations_pkey");

            entity.ToTable("conversations", "unick_schema");

            entity.Property(e => e.IdConversation).HasColumnName("id_conversation");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.IdUser1).HasColumnName("id_user_1");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.ConversationIdUserNavigations)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("conversations_id_user_fkey");

            entity.HasOne(d => d.IdUser1Navigation).WithMany(p => p.ConversationIdUser1Navigations)
                .HasForeignKey(d => d.IdUser1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("conversations_id_user_1_fkey");
        });

        modelBuilder.Entity<Crowdfundingtag>(entity =>
        {
            entity.HasKey(e => e.IdCrowfundingTag).HasName("crowdfundingtags_pkey");

            entity.ToTable("crowdfundingtags", "unick_schema");

            entity.Property(e => e.IdCrowfundingTag).HasColumnName("id_crowfunding_tag");
            entity.Property(e => e.IdProject)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_project");
            entity.Property(e => e.IdTag).HasColumnName("id_tag");

            entity.HasOne(d => d.IdProjectNavigation).WithMany(p => p.Crowdfundingtags)
                .HasForeignKey(d => d.IdProject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("crowdfundingtags_id_project_fkey");

            entity.HasOne(d => d.IdTagNavigation).WithMany(p => p.Crowdfundingtags)
                .HasForeignKey(d => d.IdTag)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("crowdfundingtags_id_tag_fkey");
        });

        modelBuilder.Entity<Donation>(entity =>
        {
            entity.HasKey(e => e.IdDonate).HasName("donations_pkey");

            entity.ToTable("donations", "unick_schema");

            entity.Property(e => e.IdDonate).HasColumnName("id_donate");
            entity.Property(e => e.IdProject).HasColumnName("id_project");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Quantity)
                .HasColumnType("money")
                .HasColumnName("quantity");

            entity.HasOne(d => d.IdProjectNavigation).WithMany(p => p.Donations)
                .HasForeignKey(d => d.IdProject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("donations_id_project_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Donations)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("donations_id_user_fkey");
        });

        modelBuilder.Entity<Favouritelot>(entity =>
        {
            entity.HasKey(e => e.IdFavouriteLot).HasName("favouritelots_pkey");

            entity.ToTable("favouritelots", "unick_schema");

            entity.Property(e => e.IdFavouriteLot).HasColumnName("id_favourite_lot");
            entity.Property(e => e.IdLot).HasColumnName("id_lot");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdLotNavigation).WithMany(p => p.Favouritelots)
                .HasForeignKey(d => d.IdLot)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("favouritelots_id_lot_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Favouritelots)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("favouritelots_id_user_fkey");
        });

        modelBuilder.Entity<Favouritetag>(entity =>
        {
            entity.HasKey(e => e.IdFavouriteTag).HasName("favouritetags_pkey");

            entity.ToTable("favouritetags", "unick_schema");

            entity.Property(e => e.IdFavouriteTag).HasColumnName("id_favourite_tag");
            entity.Property(e => e.IdTag).HasColumnName("id_tag");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdTagNavigation).WithMany(p => p.Favouritetags)
                .HasForeignKey(d => d.IdTag)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("favouritetags_id_tag_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Favouritetags)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("favouritetags_id_user_fkey");
        });

        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.IdFile).HasName("id_file");

            entity.ToTable("files", "unick_schema");

            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Path).HasColumnName("path");
        });


            modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.IdMessage).HasName("messages_pkey");

            entity.ToTable("messages", "unick_schema");

            entity.Property(e => e.IdMessage).HasColumnName("id_message");
            entity.Property(e => e.IdConversation)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_conversation");
            entity.Property(e => e.Text)
                .HasMaxLength(1000)
                .HasColumnName("text");
            entity.Property(e => e.idSender).HasColumnName("id_sender");
            

            entity.HasOne(d => d.IdConversationNavigation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.IdConversation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("messages_id_conversation_fkey");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.IdProject).HasName("projects_pkey");

            entity.ToTable("projects", "unick_schema");

            entity.Property(e => e.IdProject).HasColumnName("id_project");
            entity.Property(e => e.Category)
                .HasMaxLength(15)
                .HasColumnName("category");
            entity.Property(e => e.CoverLink).HasColumnName("cover_link");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.DonateAmount)
                .HasColumnType("money")
                .HasColumnName("donate_amount");
            entity.Property(e => e.EndDateDonate).HasColumnName("end_date_donate");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.StartDateDonate).HasColumnName("start_date_donate");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(30)
                .HasColumnName("title");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Projects)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projects_id_user_fkey");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.IdTag).HasName("tags_pkey");

            entity.ToTable("tags", "unick_schema");

            entity.Property(e => e.IdTag).HasColumnName("id_tag");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.IdTransaction).HasName("transactions_pkey");

            entity.ToTable("transactions", "unick_schema");

            entity.Property(e => e.IdTransaction).HasColumnName("id_transaction");
            entity.Property(e => e.IdReceiver).HasColumnName("id_receiver");
            entity.Property(e => e.IdSender).HasColumnName("id_sender");
            entity.Property(e => e.Quantity)
                .HasColumnType("money")
                .HasColumnName("quantity");

            entity.HasOne(d => d.IdSenderNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.IdSender)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("transactions_id_sender_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("users_pkey");

            entity.ToTable("users", "unick_schema");

            entity.HasIndex(e => e.Login, "users_login_key").IsUnique();

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.DateOfRegistration).HasColumnName("date_of_registration");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .HasColumnName("first_name");
            entity.Property(e => e.IdProject).HasColumnName("id_project");
            entity.Property(e => e.IdSupportProject).HasColumnName("id_support_project");
            entity.Property(e => e.IdWallet)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_wallet");
            entity.Property(e => e.Login)
                .HasMaxLength(30)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(64)
                .IsFixedLength()
                .HasColumnName("password");
            entity.Property(e => e.SecondName)
                .HasMaxLength(20)
                .HasColumnName("second_name");
            entity.Property(e => e.Role)
                .HasColumnName("role");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.IdWallet).HasName("wallets_pkey");

            entity.ToTable("wallets", "unick_schema");

            entity.Property(e => e.IdWallet).HasColumnName("id_wallet");
            entity.Property(e => e.Balance)
                .HasColumnType("money")
                .HasColumnName("balance");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("wallets_id_user_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
