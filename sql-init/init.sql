USE [_API_shop]
GO
/****** Object:  Table [dbo].[CATEGORIES]    Script Date: 20/05/2026 15:14:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CATEGORIES](
	[CATEGORY_ID] [int] IDENTITY(1,1) NOT NULL,
	[CATEGORY_NAME] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_CATEGORIES] PRIMARY KEY CLUSTERED 
(
	[CATEGORY_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ORDER_ITEM]    Script Date: 20/05/2026 15:14:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ORDER_ITEM](
	[ORDER_ITEM_ID] [int] IDENTITY(1,1) NOT NULL,
	[PRODUCT_ID] [int] NOT NULL,
	[ORDER_ID] [int] NOT NULL,
	[QUANTITY] [int] NOT NULL,
 CONSTRAINT [PK_ORDER_ITEM] PRIMARY KEY CLUSTERED 
(
	[ORDER_ITEM_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ORDERS]    Script Date: 20/05/2026 15:14:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ORDERS](
	[ORDER_ID] [int] IDENTITY(1,1) NOT NULL,
	[ORDER_DATE] [date] NOT NULL,
	[ORDER_SUM] [float] NOT NULL,
	[USER_ID] [int] NOT NULL,
 CONSTRAINT [PK_ORDERS] PRIMARY KEY CLUSTERED 
(
	[ORDER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PRODUCTS]    Script Date: 20/05/2026 15:14:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PRODUCTS](
	[PRODUCT_ID] [int] IDENTITY(1,1) NOT NULL,
	[PRODUCT_NAME] [nvarchar](50) NOT NULL,
	[PRICE] [float] NOT NULL,
	[CATEGORY_ID] [int] NOT NULL,
	[DESCRIPTION] [nvarchar](max) NULL,
 CONSTRAINT [PK_PRODUCTS] PRIMARY KEY CLUSTERED 
(
	[PRODUCT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RATING]    Script Date: 20/05/2026 15:14:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RATING](
	[RATING_ID] [int] IDENTITY(1,1) NOT NULL,
	[HOST] [nvarchar](50) NULL,
	[METHOD] [nchar](10) NULL,
	[PATH] [nvarchar](50) NULL,
	[REFERER] [nvarchar](100) NULL,
	[USER_AGENT] [nvarchar](max) NULL,
	[Record_Date] [datetime] NULL,
 CONSTRAINT [PK_RATING] PRIMARY KEY CLUSTERED 
(
	[RATING_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 20/05/2026 15:14:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[userId] [int] IDENTITY(1,1) NOT NULL,
	[userEmail] [varchar](50) NOT NULL,
	[userFirstName] [varchar](50) NULL,
	[userLastName] [varchar](50) NULL,
	[password] [varchar](60) NOT NULL,
	[isAdmin] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CATEGORIES] ON 

INSERT [dbo].[CATEGORIES] ([CATEGORY_ID], [CATEGORY_NAME]) VALUES (1, N'לבית ולמטבח')
INSERT [dbo].[CATEGORIES] ([CATEGORY_ID], [CATEGORY_NAME]) VALUES (2, N'טקסטיל')
INSERT [dbo].[CATEGORIES] ([CATEGORY_ID], [CATEGORY_NAME]) VALUES (3, N'אופנה')
INSERT [dbo].[CATEGORIES] ([CATEGORY_ID], [CATEGORY_NAME]) VALUES (4, N'אביזרים וציוד משרדי')
INSERT [dbo].[CATEGORIES] ([CATEGORY_ID], [CATEGORY_NAME]) VALUES (5, N'פנאי ונוי')
SET IDENTITY_INSERT [dbo].[CATEGORIES] OFF
GO
SET IDENTITY_INSERT [dbo].[ORDER_ITEM] ON 

INSERT [dbo].[ORDER_ITEM] ([ORDER_ITEM_ID], [PRODUCT_ID], [ORDER_ID], [QUANTITY]) VALUES (13, 5, 28, 5)
INSERT [dbo].[ORDER_ITEM] ([ORDER_ITEM_ID], [PRODUCT_ID], [ORDER_ID], [QUANTITY]) VALUES (14, 9, 29, 2)
INSERT [dbo].[ORDER_ITEM] ([ORDER_ITEM_ID], [PRODUCT_ID], [ORDER_ID], [QUANTITY]) VALUES (15, 14, 30, 6)
INSERT [dbo].[ORDER_ITEM] ([ORDER_ITEM_ID], [PRODUCT_ID], [ORDER_ID], [QUANTITY]) VALUES (16, 24, 30, 3)
SET IDENTITY_INSERT [dbo].[ORDER_ITEM] OFF
GO
SET IDENTITY_INSERT [dbo].[ORDERS] ON 

INSERT [dbo].[ORDERS] ([ORDER_ID], [ORDER_DATE], [ORDER_SUM], [USER_ID]) VALUES (28, CAST(N'2026-05-20' AS Date), 325, 42)
INSERT [dbo].[ORDERS] ([ORDER_ID], [ORDER_DATE], [ORDER_SUM], [USER_ID]) VALUES (29, CAST(N'2026-05-20' AS Date), 220, 43)
INSERT [dbo].[ORDERS] ([ORDER_ID], [ORDER_DATE], [ORDER_SUM], [USER_ID]) VALUES (30, CAST(N'2026-05-20' AS Date), 660, 43)
SET IDENTITY_INSERT [dbo].[ORDERS] OFF
GO
SET IDENTITY_INSERT [dbo].[PRODUCTS] ON 

INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (1, N'קרש חיתוך מעץ אלון יוקרתי', 149, 1, N' קרש חיתוך עמיד ואיכותי העשוי מעץ אלון מלא, עליו ניתן לחרוט בלייזר שמות משפחה, מתכונים נוסטלגיים או הקדשות אישיות. זהו פתרון מושלם להגשה מרשימה של גבינות ואירוח, המשלב פרקטיקה עם נגיעה עיצובית חמה במטבח')
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (2, N'סט תחתיות לכוסות משעם', 45, 1, N' סט של 6 תחתיות טבעיות המונעות סימני רטיבות על השולחן, עם אפשרות להדפסת משפטי השראה או שמות האורחים על כל תחתית. עיצוב מינימליסטי שמוסיף אופי לכל פינת אוכל או סלון')
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (3, N'ספל מאג קרמי עם כיתוב מוזהב', 39, 1, N'ספל קפה קלאסי בגימור מט, עליו ניתן להדפיס שם או משפט בוקר טוב באותיות זהב בולטות ויוקרתיות. המתנה האידיאלית למי שרוצה לפתוח את היום עם חיוך ומסר אישי על השולחן')
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (4, N'שטיח כניסה לבית בעיצוב אישי', 89, 1, N'שטיח "ברוכים הבאים" עשוי סיבי קוקוס איכותיים, המאפשר להוסיף את שם המשפחה או משפט מקורי שמקבל את פני האורחים. השטיח עמיד בתנאי חוץ ושומר על ניקיון הבית בסטייל ייחודי')
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (5, N'קופסת עץ ליין עם חריטה', 65, 2, N'קופסת אחסון מהודרת לבקבוק יין, הכוללת חריטה של תאריך יום ההולדת וברכה אישית מעומק הלב. זוהי דרך יוקרתית להפוך מתנה סטנדרטית למזכרת שתישאר לשנים רבות')
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (8, N'בלון גז קריסטלי עם כיתוב', 55, 2, N'בלון שקוף וגדול הממולא בקונפטי צבעוני, ועליו מודפס שם חתן או כלה השמחה בכתב יד מעוצב. פתרון דקורטיבי ומרהיב שמוסיף אווירה חגיגית לכל מסיבת יום הולדת')
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (9, N'פאזל עץ "סיבות למה אני אוהב אותך"', 110, 2, N' פאזל ייחודי שבו על כל חלק נחטרת סיבה אחרת או זיכרון משותף, ליצירת חוויה מרגשת של הרכבה וגילוי. מתנה אישית מאוד שמתאימה לימי הולדת עגולים או לבני זוג')
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (10, N'נר ריחני בכלי זכוכית עם הקדשה', 49, 2, N'נר ריחני בכלי זכוכית עם הקדשה')
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (11, N'עט מתכת יוקרתי עם חריטת שם', 79, 3, N'עט נובע או עט כדורי בגימור כרום, הכולל חריטת לייזר מדויקת של שם המשתמש על גוף העט. מתנה פרקטית ומכובדת למנהלים, סטודנטים או לכל מי שמעריך כלי כתיבה איכותיים')
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (12, N'מחברת פרימיום עם כריכה קשה', 59, 3, N'מחברת דפים שורות עם כריכת דמוי עור איכותית, עליה ניתן להטביע שם או משפט מוטיבציה. מושלמת לרישום פרוטוקולים, רעיונות יצירתיים או לניהול משימות יומי')
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (13, N'משטח לעכבר (Mouse Pad) ממותג', 35, 3, N'משטח ארגונומי רך המאפשר עבודה חלקה עם העכבר, עליו ניתן להדפיס את התפקיד שלכם או משפט מצחיק למשרד. משדרג את נראות שולחן העבודה ומוסיף צבע לסביבת העבודה')
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (14, N'מעמד שולחני לטלפון מעץ', 45, 3, N'סטנד מעוצב לטלפון הנייד המאפשר צפייה נוחה במסך תוך כדי עבודה, עם חריטה של ראשי תיבות או שם החברה. מוצר המשלב סדר על השולחן עם נגיעה אישית חמימה')
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (15, N'מגבת גוף עם רקמה אישית', 75, 4, N'מגבת רחצה גדולה ומפנקת עשויה 100% כותנה איכותית, הכוללת רקמת שם בולטת במגוון צבעים לבחירה. מתנה נהדרת למתגייסים, למטיילים או כפינוק אישי לבית')
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (18, N'ציפית לכרית עם משפט "לילה טוב"', 40, 4, N'ציפית רכה ונעימה למגע עליה ניתן להדפיס הקדשה אישית או שם, כדי להפוך את השינה לנעימה יותר. דרך יצירתית להוסיף חום ואישיות לחדר השינה')
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (19, N'חלוק רחצה מפנק עם רקמה', 180, 4, N'חלוק מגבת רך וסופג המעניק תחושת ספא ביתית, עם רקמת שם או ראשי תיבות על גב החלוק או על הכיס. מוצר פופולרי מאוד כמתנת חתונה או יום נישואין')
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (20, N'סינר מטבח בעיצוב אישי', 65, 4, N' סינר בד עמיד המגן על הבגדים בזמן הבישול, עם הדפסת טקסט כמו "השף של הבית" או "המטבח של אמא". מתנה משעשעת ושימושית לכל חובב קולינריה')
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (21, N'חולצת T-shirt עם כיתוב מקורי', 55, 5, N'חולצת כותנה בגזרה נוחה עליה ניתן להדפיס כל טקסט שעולה בדעתכם, מציטוטים של שירים ועד בדיחות פנימיות. הבסיס המושלם ליצירת אמירה אופנתית אישית וייחודית')
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (22, N'כובע מצחייה (Cap) עם רקמה', 50, 5, N'כובע איכותי עם סגירה מתכווננת, המאפשר רקמת שם או מילה קצרה בחזית הכובע. אקססורי מושלם לימי הקיץ שמוסיף סטייל אישי לכל הופעה יומיומית')
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (23, N'תיק בד (Tote Bag) ממותג', 30, 5, N'תיק בד רב-פעמי וידידותי לסביבה, עליו ניתן להדפיס משפטים מעוררי השראה או שמות מעוצבים. פתרון אופנתי ונוח לנשיאת קניות, ספרים או ציוד לים')
INSERT [dbo].[PRODUCTS] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION]) VALUES (24, N'ארנק עור עם חריטה פנימית', 130, 5, N'ארנק גברים קלאסי מעור אמיתי, המאפשר לחרוט הקדשה אישית נסתרת בחלקו הפנימי. מתנה אינטימית ומרגשת שזוכים לראות בכל פעם שפותחים את הארנק')
SET IDENTITY_INSERT [dbo].[PRODUCTS] OFF
GO
SET IDENTITY_INSERT [dbo].[RATING] ON 

INSERT [dbo].[RATING] ([RATING_ID], [HOST], [METHOD], [PATH], [REFERER], [USER_AGENT], [Record_Date]) VALUES (615, N'localhost:44324', N'PUT       ', N'/api/Users/43', N'https://localhost:44324/update.html', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36', CAST(N'2026-05-20T14:51:36.230' AS DateTime))
INSERT [dbo].[RATING] ([RATING_ID], [HOST], [METHOD], [PATH], [REFERER], [USER_AGENT], [Record_Date]) VALUES (616, N'localhost:44324', N'GET       ', N'/api/Orders', N'https://localhost:44324/SWAGGER/index.html', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36', CAST(N'2026-05-20T14:52:21.643' AS DateTime))
INSERT [dbo].[RATING] ([RATING_ID], [HOST], [METHOD], [PATH], [REFERER], [USER_AGENT], [Record_Date]) VALUES (617, N'localhost:44324', N'POST      ', N'/api/Users/login', N'https://localhost:44324/home.html', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36', CAST(N'2026-05-20T14:52:53.483' AS DateTime))
INSERT [dbo].[RATING] ([RATING_ID], [HOST], [METHOD], [PATH], [REFERER], [USER_AGENT], [Record_Date]) VALUES (618, N'localhost:44324', N'GET       ', N'/api/Orders', N'https://localhost:44324/SWAGGER/index.html', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36', CAST(N'2026-05-20T14:53:20.373' AS DateTime))
INSERT [dbo].[RATING] ([RATING_ID], [HOST], [METHOD], [PATH], [REFERER], [USER_AGENT], [Record_Date]) VALUES (619, N'localhost:44324', N'POST      ', N'/api/Orders', N'https://localhost:44324/SWAGGER/index.html', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36', CAST(N'2026-05-20T14:54:10.187' AS DateTime))
INSERT [dbo].[RATING] ([RATING_ID], [HOST], [METHOD], [PATH], [REFERER], [USER_AGENT], [Record_Date]) VALUES (620, N'localhost:44324', N'POST      ', N'/api/Orders', N'https://localhost:44324/SWAGGER/index.html', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36', CAST(N'2026-05-20T14:54:25.547' AS DateTime))
INSERT [dbo].[RATING] ([RATING_ID], [HOST], [METHOD], [PATH], [REFERER], [USER_AGENT], [Record_Date]) VALUES (621, N'localhost:44324', N'POST      ', N'/api/Orders', N'https://localhost:44324/SWAGGER/index.html', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36', CAST(N'2026-05-20T14:55:02.097' AS DateTime))
INSERT [dbo].[RATING] ([RATING_ID], [HOST], [METHOD], [PATH], [REFERER], [USER_AGENT], [Record_Date]) VALUES (622, N'localhost:44324', N'GET       ', N'/api/Orders', N'https://localhost:44324/SWAGGER/index.html', N'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36', CAST(N'2026-05-20T14:55:10.533' AS DateTime))
SET IDENTITY_INSERT [dbo].[RATING] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([userId], [userEmail], [userFirstName], [userLastName], [password], [isAdmin]) VALUES (42, N'LaliG@gmail.com', N'Lali', N'Gosh', N'$2a$11$p2PM6OGx7G/uNFJxuA2Wm.rdR9T9xhfXHBKYEfe4iUZZQeGOkoUgm', 1)
INSERT [dbo].[User] ([userId], [userEmail], [userFirstName], [userLastName], [password], [isAdmin]) VALUES (43, N'sara.cohen@example.com', N'Sara', N'Cohen', N'$2a$11$SRkLBjdkQ2N547ZnkgJlkOHJI0MFHZdy1bUa286eFkVNcgyM1uDcK', 0)
SET IDENTITY_INSERT [dbo].[User] OFF
GO
ALTER TABLE [dbo].[User] ADD  DEFAULT ((0)) FOR [isAdmin]
GO
ALTER TABLE [dbo].[ORDER_ITEM]  WITH CHECK ADD  CONSTRAINT [FK_ORDER_ITEM_ORDERS] FOREIGN KEY([ORDER_ID])
REFERENCES [dbo].[ORDERS] ([ORDER_ID])
GO
ALTER TABLE [dbo].[ORDER_ITEM] CHECK CONSTRAINT [FK_ORDER_ITEM_ORDERS]
GO
ALTER TABLE [dbo].[ORDER_ITEM]  WITH CHECK ADD  CONSTRAINT [FK_ORDER_ITEM_PRODUCTS] FOREIGN KEY([PRODUCT_ID])
REFERENCES [dbo].[PRODUCTS] ([PRODUCT_ID])
GO
ALTER TABLE [dbo].[ORDER_ITEM] CHECK CONSTRAINT [FK_ORDER_ITEM_PRODUCTS]
GO
ALTER TABLE [dbo].[ORDERS]  WITH CHECK ADD  CONSTRAINT [FK_ORDERS_User] FOREIGN KEY([USER_ID])
REFERENCES [dbo].[User] ([userId])
GO
ALTER TABLE [dbo].[ORDERS] CHECK CONSTRAINT [FK_ORDERS_User]
GO
ALTER TABLE [dbo].[PRODUCTS]  WITH CHECK ADD  CONSTRAINT [FK_PRODUCTS_CATEGORIES] FOREIGN KEY([CATEGORY_ID])
REFERENCES [dbo].[CATEGORIES] ([CATEGORY_ID])
GO
ALTER TABLE [dbo].[PRODUCTS] CHECK CONSTRAINT [FK_PRODUCTS_CATEGORIES]
GO
