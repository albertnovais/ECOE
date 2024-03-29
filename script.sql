/****** Object:  Database [Ecoe]    Script Date: 07/10/2019 20:17:51 ******/
CREATE DATABASE [Ecoe]  (EDITION = 'Standard', SERVICE_OBJECTIVE = 'S0', MAXSIZE = 250 GB) WITH CATALOG_COLLATION = SQL_Latin1_General_CP1_CI_AS;
GO
ALTER DATABASE [Ecoe] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Ecoe] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Ecoe] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Ecoe] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Ecoe] SET ARITHABORT OFF 
GO
ALTER DATABASE [Ecoe] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Ecoe] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Ecoe] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Ecoe] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Ecoe] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Ecoe] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Ecoe] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Ecoe] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Ecoe] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [Ecoe] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Ecoe] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [Ecoe] SET  MULTI_USER 
GO
ALTER DATABASE [Ecoe] SET ENCRYPTION ON
GO
ALTER DATABASE [Ecoe] SET QUERY_STORE = ON
GO
ALTER DATABASE [Ecoe] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 100, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO)
GO
/****** Object:  UserDefinedFunction [dbo].[fn_diagramobjects]    Script Date: 07/10/2019 20:17:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE FUNCTION [dbo].[fn_diagramobjects]() 
	RETURNS int
	WITH EXECUTE AS N'dbo'
	AS
	BEGIN
		declare @id_upgraddiagrams		int
		declare @id_sysdiagrams			int
		declare @id_helpdiagrams		int
		declare @id_helpdiagramdefinition	int
		declare @id_creatediagram	int
		declare @id_renamediagram	int
		declare @id_alterdiagram 	int 
		declare @id_dropdiagram		int
		declare @InstalledObjects	int

		select @InstalledObjects = 0

		select 	@id_upgraddiagrams = object_id(N'dbo.sp_upgraddiagrams'),
			@id_sysdiagrams = object_id(N'dbo.sysdiagrams'),
			@id_helpdiagrams = object_id(N'dbo.sp_helpdiagrams'),
			@id_helpdiagramdefinition = object_id(N'dbo.sp_helpdiagramdefinition'),
			@id_creatediagram = object_id(N'dbo.sp_creatediagram'),
			@id_renamediagram = object_id(N'dbo.sp_renamediagram'),
			@id_alterdiagram = object_id(N'dbo.sp_alterdiagram'), 
			@id_dropdiagram = object_id(N'dbo.sp_dropdiagram')

		if @id_upgraddiagrams is not null
			select @InstalledObjects = @InstalledObjects + 1
		if @id_sysdiagrams is not null
			select @InstalledObjects = @InstalledObjects + 2
		if @id_helpdiagrams is not null
			select @InstalledObjects = @InstalledObjects + 4
		if @id_helpdiagramdefinition is not null
			select @InstalledObjects = @InstalledObjects + 8
		if @id_creatediagram is not null
			select @InstalledObjects = @InstalledObjects + 16
		if @id_renamediagram is not null
			select @InstalledObjects = @InstalledObjects + 32
		if @id_alterdiagram  is not null
			select @InstalledObjects = @InstalledObjects + 64
		if @id_dropdiagram is not null
			select @InstalledObjects = @InstalledObjects + 128
		
		return @InstalledObjects 
	END
	
GO
/****** Object:  Table [dbo].[Acesso]    Script Date: 07/10/2019 20:17:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Acesso](
	[AcessoId] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](100) NOT NULL,
	[Descricao] [text] NULL,
 CONSTRAINT [PK_Acesso] PRIMARY KEY CLUSTERED 
(
	[AcessoId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlunoAvaliacao]    Script Date: 07/10/2019 20:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlunoAvaliacao](
	[PessoaId] [int] NOT NULL,
	[AvaliacaoId] [int] NOT NULL,
	[PessoaCadastrou] [int] NOT NULL,
	[DataCadastro] [datetime] NOT NULL,
	[DuplaId] [int] NULL,
 CONSTRAINT [PK_AlunoAvaliacao] PRIMARY KEY CLUSTERED 
(
	[PessoaId] ASC,
	[AvaliacaoId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlunoQuestao]    Script Date: 07/10/2019 20:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlunoQuestao](
	[PessoaId] [int] NOT NULL,
	[DataHora] [datetime] NOT NULL,
	[Nota] [float] NOT NULL,
	[QuestaoId] [int] NOT NULL,
	[AvaliadorId] [int] NOT NULL,
 CONSTRAINT [PK_AlunoQuestao_1] PRIMARY KEY CLUSTERED 
(
	[PessoaId] ASC,
	[QuestaoId] ASC,
	[AvaliadorId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Avaliacoes]    Script Date: 07/10/2019 20:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Avaliacoes](
	[AvaliacaoId] [int] IDENTITY(1,1) NOT NULL,
	[PessoaId] [int] NOT NULL,
	[Nome] [varchar](150) NOT NULL,
	[Descricao] [varchar](max) NOT NULL,
	[DataCadastro] [date] NOT NULL,
	[TurmaId] [int] NOT NULL,
	[Peso] [decimal](18, 2) NULL,
	[StatusId] [int] NULL,
	[DataAvaliacao] [date] NULL,
	[Dupla] [bit] NOT NULL,
 CONSTRAINT [PK_Avaliacoes] PRIMARY KEY CLUSTERED 
(
	[AvaliacaoId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Curso]    Script Date: 07/10/2019 20:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Curso](
	[CursoId] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](100) NOT NULL,
	[StatusId] [int] NULL,
	[PessoaId] [int] NOT NULL,
 CONSTRAINT [PK_Curso] PRIMARY KEY CLUSTERED 
(
	[CursoId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pessoa]    Script Date: 07/10/2019 20:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pessoa](
	[PessoaId] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](100) NOT NULL,
	[Email] [varchar](100) NULL,
	[Senha] [varchar](256) NULL,
	[PessoaCadastrou] [int] NOT NULL,
	[RA] [varchar](50) NULL,
	[AcessoId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
 CONSTRAINT [PK_Pessoa] PRIMARY KEY CLUSTERED 
(
	[PessoaId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PessoaCurso]    Script Date: 07/10/2019 20:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PessoaCurso](
	[PessoaId] [int] NOT NULL,
	[CursoId] [int] NOT NULL,
	[Data] [date] NOT NULL,
	[PessoaCadastrou] [int] NOT NULL,
 CONSTRAINT [PK_PessoaCurso] PRIMARY KEY CLUSTERED 
(
	[PessoaId] ASC,
	[CursoId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Questao]    Script Date: 07/10/2019 20:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Questao](
	[QuestaoId] [int] IDENTITY(1,1) NOT NULL,
	[Descricao] [varchar](max) NOT NULL,
	[DataCadastro] [date] NOT NULL,
	[PessoaId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
	[AvaliacaoId] [int] NOT NULL,
 CONSTRAINT [PK_Questao] PRIMARY KEY CLUSTERED 
(
	[QuestaoId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 07/10/2019 20:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[StatusId] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](100) NOT NULL,
	[Descricao] [text] NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sysdiagrams]    Script Date: 07/10/2019 20:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sysdiagrams](
	[name] [sysname] NOT NULL,
	[principal_id] [int] NOT NULL,
	[diagram_id] [int] IDENTITY(1,1) NOT NULL,
	[version] [int] NULL,
	[definition] [varbinary](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[diagram_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_principal_name] UNIQUE NONCLUSTERED 
(
	[principal_id] ASC,
	[name] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Turma]    Script Date: 07/10/2019 20:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Turma](
	[TurmaId] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](100) NOT NULL,
	[CursoId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
	[PessoaId] [int] NOT NULL,
	[Turno] [varchar](50) NULL,
	[Ano] [int] NULL,
	[Periodo] [int] NULL,
 CONSTRAINT [PK_Turma] PRIMARY KEY CLUSTERED 
(
	[TurmaId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TurmaPessoa]    Script Date: 07/10/2019 20:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TurmaPessoa](
	[PessoaId] [int] NOT NULL,
	[TurmaId] [int] NOT NULL,
	[pessoaCadastrou] [int] NOT NULL,
	[DataHora] [datetime] NOT NULL,
	[Observecao] [varchar](max) NULL,
 CONSTRAINT [PK_TurmaPessoa] PRIMARY KEY CLUSTERED 
(
	[PessoaId] ASC,
	[TurmaId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AlunoAvaliacao]  WITH CHECK ADD  CONSTRAINT [FK_AlunoAvaliacao_Avaliacoes] FOREIGN KEY([AvaliacaoId])
REFERENCES [dbo].[Avaliacoes] ([AvaliacaoId])
GO
ALTER TABLE [dbo].[AlunoAvaliacao] CHECK CONSTRAINT [FK_AlunoAvaliacao_Avaliacoes]
GO
ALTER TABLE [dbo].[AlunoAvaliacao]  WITH CHECK ADD  CONSTRAINT [FK_AlunoAvaliacao_Pessoa] FOREIGN KEY([PessoaId])
REFERENCES [dbo].[Pessoa] ([PessoaId])
GO
ALTER TABLE [dbo].[AlunoAvaliacao] CHECK CONSTRAINT [FK_AlunoAvaliacao_Pessoa]
GO
ALTER TABLE [dbo].[AlunoAvaliacao]  WITH CHECK ADD  CONSTRAINT [FK_AlunoAvaliacao_Pessoa1] FOREIGN KEY([DuplaId])
REFERENCES [dbo].[Pessoa] ([PessoaId])
GO
ALTER TABLE [dbo].[AlunoAvaliacao] CHECK CONSTRAINT [FK_AlunoAvaliacao_Pessoa1]
GO
ALTER TABLE [dbo].[AlunoQuestao]  WITH CHECK ADD  CONSTRAINT [FK_AlunoAlternativa_Pessoa] FOREIGN KEY([PessoaId])
REFERENCES [dbo].[Pessoa] ([PessoaId])
GO
ALTER TABLE [dbo].[AlunoQuestao] CHECK CONSTRAINT [FK_AlunoAlternativa_Pessoa]
GO
ALTER TABLE [dbo].[AlunoQuestao]  WITH CHECK ADD  CONSTRAINT [FK_AlunoQuestao_Pessoa] FOREIGN KEY([AvaliadorId])
REFERENCES [dbo].[Pessoa] ([PessoaId])
GO
ALTER TABLE [dbo].[AlunoQuestao] CHECK CONSTRAINT [FK_AlunoQuestao_Pessoa]
GO
ALTER TABLE [dbo].[AlunoQuestao]  WITH CHECK ADD  CONSTRAINT [FK_AlunoQuestao_Questao] FOREIGN KEY([QuestaoId])
REFERENCES [dbo].[Questao] ([QuestaoId])
GO
ALTER TABLE [dbo].[AlunoQuestao] CHECK CONSTRAINT [FK_AlunoQuestao_Questao]
GO
ALTER TABLE [dbo].[Avaliacoes]  WITH CHECK ADD  CONSTRAINT [FK_Avaliacoes_Pessoa] FOREIGN KEY([PessoaId])
REFERENCES [dbo].[Pessoa] ([PessoaId])
GO
ALTER TABLE [dbo].[Avaliacoes] CHECK CONSTRAINT [FK_Avaliacoes_Pessoa]
GO
ALTER TABLE [dbo].[Avaliacoes]  WITH CHECK ADD  CONSTRAINT [FK_Avaliacoes_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Avaliacoes] CHECK CONSTRAINT [FK_Avaliacoes_Status]
GO
ALTER TABLE [dbo].[Avaliacoes]  WITH CHECK ADD  CONSTRAINT [FK_Avaliacoes_Turma] FOREIGN KEY([TurmaId])
REFERENCES [dbo].[Turma] ([TurmaId])
GO
ALTER TABLE [dbo].[Avaliacoes] CHECK CONSTRAINT [FK_Avaliacoes_Turma]
GO
ALTER TABLE [dbo].[Curso]  WITH CHECK ADD  CONSTRAINT [FK_Curso_Pessoa] FOREIGN KEY([PessoaId])
REFERENCES [dbo].[Pessoa] ([PessoaId])
GO
ALTER TABLE [dbo].[Curso] CHECK CONSTRAINT [FK_Curso_Pessoa]
GO
ALTER TABLE [dbo].[Curso]  WITH CHECK ADD  CONSTRAINT [FK_Curso_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Curso] CHECK CONSTRAINT [FK_Curso_Status]
GO
ALTER TABLE [dbo].[Pessoa]  WITH CHECK ADD  CONSTRAINT [FK_Pessoa_Acesso] FOREIGN KEY([AcessoId])
REFERENCES [dbo].[Acesso] ([AcessoId])
GO
ALTER TABLE [dbo].[Pessoa] CHECK CONSTRAINT [FK_Pessoa_Acesso]
GO
ALTER TABLE [dbo].[Pessoa]  WITH CHECK ADD  CONSTRAINT [FK_Pessoa_Pessoa] FOREIGN KEY([PessoaCadastrou])
REFERENCES [dbo].[Pessoa] ([PessoaId])
GO
ALTER TABLE [dbo].[Pessoa] CHECK CONSTRAINT [FK_Pessoa_Pessoa]
GO
ALTER TABLE [dbo].[Pessoa]  WITH CHECK ADD  CONSTRAINT [FK_Pessoa_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Pessoa] CHECK CONSTRAINT [FK_Pessoa_Status]
GO
ALTER TABLE [dbo].[PessoaCurso]  WITH CHECK ADD  CONSTRAINT [FK_PessoaCurso_Curso1] FOREIGN KEY([CursoId])
REFERENCES [dbo].[Curso] ([CursoId])
GO
ALTER TABLE [dbo].[PessoaCurso] CHECK CONSTRAINT [FK_PessoaCurso_Curso1]
GO
ALTER TABLE [dbo].[PessoaCurso]  WITH CHECK ADD  CONSTRAINT [FK_PessoaCurso_Pessoa] FOREIGN KEY([PessoaId])
REFERENCES [dbo].[Pessoa] ([PessoaId])
GO
ALTER TABLE [dbo].[PessoaCurso] CHECK CONSTRAINT [FK_PessoaCurso_Pessoa]
GO
ALTER TABLE [dbo].[PessoaCurso]  WITH CHECK ADD  CONSTRAINT [FK_PessoaCurso_Pessoa1] FOREIGN KEY([PessoaCadastrou])
REFERENCES [dbo].[Pessoa] ([PessoaId])
GO
ALTER TABLE [dbo].[PessoaCurso] CHECK CONSTRAINT [FK_PessoaCurso_Pessoa1]
GO
ALTER TABLE [dbo].[Questao]  WITH CHECK ADD  CONSTRAINT [FK_Questao_Avaliacoes] FOREIGN KEY([AvaliacaoId])
REFERENCES [dbo].[Avaliacoes] ([AvaliacaoId])
GO
ALTER TABLE [dbo].[Questao] CHECK CONSTRAINT [FK_Questao_Avaliacoes]
GO
ALTER TABLE [dbo].[Questao]  WITH CHECK ADD  CONSTRAINT [FK_Questao_Pessoa] FOREIGN KEY([PessoaId])
REFERENCES [dbo].[Pessoa] ([PessoaId])
GO
ALTER TABLE [dbo].[Questao] CHECK CONSTRAINT [FK_Questao_Pessoa]
GO
ALTER TABLE [dbo].[Questao]  WITH CHECK ADD  CONSTRAINT [FK_Questao_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Questao] CHECK CONSTRAINT [FK_Questao_Status]
GO
ALTER TABLE [dbo].[Turma]  WITH CHECK ADD  CONSTRAINT [FK_Turma_Curso] FOREIGN KEY([CursoId])
REFERENCES [dbo].[Curso] ([CursoId])
GO
ALTER TABLE [dbo].[Turma] CHECK CONSTRAINT [FK_Turma_Curso]
GO
ALTER TABLE [dbo].[Turma]  WITH CHECK ADD  CONSTRAINT [FK_Turma_Pessoa] FOREIGN KEY([PessoaId])
REFERENCES [dbo].[Pessoa] ([PessoaId])
GO
ALTER TABLE [dbo].[Turma] CHECK CONSTRAINT [FK_Turma_Pessoa]
GO
ALTER TABLE [dbo].[Turma]  WITH CHECK ADD  CONSTRAINT [FK_Turma_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Turma] CHECK CONSTRAINT [FK_Turma_Status]
GO
ALTER TABLE [dbo].[TurmaPessoa]  WITH CHECK ADD  CONSTRAINT [FK_TurmaPessoa_Pessoa] FOREIGN KEY([PessoaId])
REFERENCES [dbo].[Pessoa] ([PessoaId])
GO
ALTER TABLE [dbo].[TurmaPessoa] CHECK CONSTRAINT [FK_TurmaPessoa_Pessoa]
GO
ALTER TABLE [dbo].[TurmaPessoa]  WITH CHECK ADD  CONSTRAINT [FK_TurmaPessoa_Pessoa1] FOREIGN KEY([pessoaCadastrou])
REFERENCES [dbo].[Pessoa] ([PessoaId])
GO
ALTER TABLE [dbo].[TurmaPessoa] CHECK CONSTRAINT [FK_TurmaPessoa_Pessoa1]
GO
ALTER TABLE [dbo].[TurmaPessoa]  WITH CHECK ADD  CONSTRAINT [FK_TurmaPessoa_Turma] FOREIGN KEY([TurmaId])
REFERENCES [dbo].[Turma] ([TurmaId])
GO
ALTER TABLE [dbo].[TurmaPessoa] CHECK CONSTRAINT [FK_TurmaPessoa_Turma]
GO
/****** Object:  StoredProcedure [dbo].[sp_alterdiagram]    Script Date: 07/10/2019 20:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE PROCEDURE [dbo].[sp_alterdiagram]
	(
		@diagramname 	sysname,
		@owner_id	int	= null,
		@version 	int,
		@definition 	varbinary(max)
	)
	WITH EXECUTE AS 'dbo'
	AS
	BEGIN
		set nocount on
	
		declare @theId 			int
		declare @retval 		int
		declare @IsDbo 			int
		
		declare @UIDFound 		int
		declare @DiagId			int
		declare @ShouldChangeUID	int
	
		if(@diagramname is null)
		begin
			RAISERROR ('Invalid ARG', 16, 1)
			return -1
		end
	
		execute as caller;
		select @theId = DATABASE_PRINCIPAL_ID();	 
		select @IsDbo = IS_MEMBER(N'db_owner'); 
		if(@owner_id is null)
			select @owner_id = @theId;
		revert;
	
		select @ShouldChangeUID = 0
		select @DiagId = diagram_id, @UIDFound = principal_id from dbo.sysdiagrams where principal_id = @owner_id and name = @diagramname 
		
		if(@DiagId IS NULL or (@IsDbo = 0 and @theId <> @UIDFound))
		begin
			RAISERROR ('Diagram does not exist or you do not have permission.', 16, 1);
			return -3
		end
	
		if(@IsDbo <> 0)
		begin
			if(@UIDFound is null or USER_NAME(@UIDFound) is null) -- invalid principal_id
			begin
				select @ShouldChangeUID = 1 ;
			end
		end

		-- update dds data			
		update dbo.sysdiagrams set definition = @definition where diagram_id = @DiagId ;

		-- change owner
		if(@ShouldChangeUID = 1)
			update dbo.sysdiagrams set principal_id = @theId where diagram_id = @DiagId ;

		-- update dds version
		if(@version is not null)
			update dbo.sysdiagrams set version = @version where diagram_id = @DiagId ;

		return 0
	END
	
GO
/****** Object:  StoredProcedure [dbo].[sp_creatediagram]    Script Date: 07/10/2019 20:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE PROCEDURE [dbo].[sp_creatediagram]
	(
		@diagramname 	sysname,
		@owner_id		int	= null, 	
		@version 		int,
		@definition 	varbinary(max)
	)
	WITH EXECUTE AS 'dbo'
	AS
	BEGIN
		set nocount on
	
		declare @theId int
		declare @retval int
		declare @IsDbo	int
		declare @userName sysname
		if(@version is null or @diagramname is null)
		begin
			RAISERROR (N'E_INVALIDARG', 16, 1);
			return -1
		end
	
		execute as caller;
		select @theId = DATABASE_PRINCIPAL_ID(); 
		select @IsDbo = IS_MEMBER(N'db_owner');
		revert; 
		
		if @owner_id is null
		begin
			select @owner_id = @theId;
		end
		else
		begin
			if @theId <> @owner_id
			begin
				if @IsDbo = 0
				begin
					RAISERROR (N'E_INVALIDARG', 16, 1);
					return -1
				end
				select @theId = @owner_id
			end
		end
		-- next 2 line only for test, will be removed after define name unique
		if EXISTS(select diagram_id from dbo.sysdiagrams where principal_id = @theId and name = @diagramname)
		begin
			RAISERROR ('The name is already used.', 16, 1);
			return -2
		end
	
		insert into dbo.sysdiagrams(name, principal_id , version, definition)
				VALUES(@diagramname, @theId, @version, @definition) ;
		
		select @retval = @@IDENTITY 
		return @retval
	END
	
GO
/****** Object:  StoredProcedure [dbo].[sp_dropdiagram]    Script Date: 07/10/2019 20:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE PROCEDURE [dbo].[sp_dropdiagram]
	(
		@diagramname 	sysname,
		@owner_id	int	= null
	)
	WITH EXECUTE AS 'dbo'
	AS
	BEGIN
		set nocount on
		declare @theId 			int
		declare @IsDbo 			int
		
		declare @UIDFound 		int
		declare @DiagId			int
	
		if(@diagramname is null)
		begin
			RAISERROR ('Invalid value', 16, 1);
			return -1
		end
	
		EXECUTE AS CALLER;
		select @theId = DATABASE_PRINCIPAL_ID();
		select @IsDbo = IS_MEMBER(N'db_owner'); 
		if(@owner_id is null)
			select @owner_id = @theId;
		REVERT; 
		
		select @DiagId = diagram_id, @UIDFound = principal_id from dbo.sysdiagrams where principal_id = @owner_id and name = @diagramname 
		if(@DiagId IS NULL or (@IsDbo = 0 and @UIDFound <> @theId))
		begin
			RAISERROR ('Diagram does not exist or you do not have permission.', 16, 1)
			return -3
		end
	
		delete from dbo.sysdiagrams where diagram_id = @DiagId;
	
		return 0;
	END
	
GO
/****** Object:  StoredProcedure [dbo].[sp_helpdiagramdefinition]    Script Date: 07/10/2019 20:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE PROCEDURE [dbo].[sp_helpdiagramdefinition]
	(
		@diagramname 	sysname,
		@owner_id	int	= null 		
	)
	WITH EXECUTE AS N'dbo'
	AS
	BEGIN
		set nocount on

		declare @theId 		int
		declare @IsDbo 		int
		declare @DiagId		int
		declare @UIDFound	int
	
		if(@diagramname is null)
		begin
			RAISERROR (N'E_INVALIDARG', 16, 1);
			return -1
		end
	
		execute as caller;
		select @theId = DATABASE_PRINCIPAL_ID();
		select @IsDbo = IS_MEMBER(N'db_owner');
		if(@owner_id is null)
			select @owner_id = @theId;
		revert; 
	
		select @DiagId = diagram_id, @UIDFound = principal_id from dbo.sysdiagrams where principal_id = @owner_id and name = @diagramname;
		if(@DiagId IS NULL or (@IsDbo = 0 and @UIDFound <> @theId ))
		begin
			RAISERROR ('Diagram does not exist or you do not have permission.', 16, 1);
			return -3
		end

		select version, definition FROM dbo.sysdiagrams where diagram_id = @DiagId ; 
		return 0
	END
	
GO
/****** Object:  StoredProcedure [dbo].[sp_helpdiagrams]    Script Date: 07/10/2019 20:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE PROCEDURE [dbo].[sp_helpdiagrams]
	(
		@diagramname sysname = NULL,
		@owner_id int = NULL
	)
	WITH EXECUTE AS N'dbo'
	AS
	BEGIN
		DECLARE @user sysname
		DECLARE @dboLogin bit
		EXECUTE AS CALLER;
			SET @user = USER_NAME();
			SET @dboLogin = CONVERT(bit,IS_MEMBER('db_owner'));
		REVERT;
		SELECT
			[Database] = DB_NAME(),
			[Name] = name,
			[ID] = diagram_id,
			[Owner] = USER_NAME(principal_id),
			[OwnerID] = principal_id
		FROM
			sysdiagrams
		WHERE
			(@dboLogin = 1 OR USER_NAME(principal_id) = @user) AND
			(@diagramname IS NULL OR name = @diagramname) AND
			(@owner_id IS NULL OR principal_id = @owner_id)
		ORDER BY
			4, 5, 1
	END
	
GO
/****** Object:  StoredProcedure [dbo].[sp_renamediagram]    Script Date: 07/10/2019 20:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE PROCEDURE [dbo].[sp_renamediagram]
	(
		@diagramname 		sysname,
		@owner_id		int	= null,
		@new_diagramname	sysname
	
	)
	WITH EXECUTE AS 'dbo'
	AS
	BEGIN
		set nocount on
		declare @theId 			int
		declare @IsDbo 			int
		
		declare @UIDFound 		int
		declare @DiagId			int
		declare @DiagIdTarg		int
		declare @u_name			sysname
		if((@diagramname is null) or (@new_diagramname is null))
		begin
			RAISERROR ('Invalid value', 16, 1);
			return -1
		end
	
		EXECUTE AS CALLER;
		select @theId = DATABASE_PRINCIPAL_ID();
		select @IsDbo = IS_MEMBER(N'db_owner'); 
		if(@owner_id is null)
			select @owner_id = @theId;
		REVERT;
	
		select @u_name = USER_NAME(@owner_id)
	
		select @DiagId = diagram_id, @UIDFound = principal_id from dbo.sysdiagrams where principal_id = @owner_id and name = @diagramname 
		if(@DiagId IS NULL or (@IsDbo = 0 and @UIDFound <> @theId))
		begin
			RAISERROR ('Diagram does not exist or you do not have permission.', 16, 1)
			return -3
		end
	
		-- if((@u_name is not null) and (@new_diagramname = @diagramname))	-- nothing will change
		--	return 0;
	
		if(@u_name is null)
			select @DiagIdTarg = diagram_id from dbo.sysdiagrams where principal_id = @theId and name = @new_diagramname
		else
			select @DiagIdTarg = diagram_id from dbo.sysdiagrams where principal_id = @owner_id and name = @new_diagramname
	
		if((@DiagIdTarg is not null) and  @DiagId <> @DiagIdTarg)
		begin
			RAISERROR ('The name is already used.', 16, 1);
			return -2
		end		
	
		if(@u_name is null)
			update dbo.sysdiagrams set [name] = @new_diagramname, principal_id = @theId where diagram_id = @DiagId
		else
			update dbo.sysdiagrams set [name] = @new_diagramname where diagram_id = @DiagId
		return 0
	END
	
GO
/****** Object:  StoredProcedure [dbo].[sp_upgraddiagrams]    Script Date: 07/10/2019 20:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE PROCEDURE [dbo].[sp_upgraddiagrams]
	AS
	BEGIN
		IF OBJECT_ID(N'dbo.sysdiagrams') IS NOT NULL
			return 0;
	
		CREATE TABLE dbo.sysdiagrams
		(
			name sysname NOT NULL,
			principal_id int NOT NULL,	-- we may change it to varbinary(85)
			diagram_id int PRIMARY KEY IDENTITY,
			version int,
	
			definition varbinary(max)
			CONSTRAINT UK_principal_name UNIQUE
			(
				principal_id,
				name
			)
		);


		/* Add this if we need to have some form of extended properties for diagrams */
		/*
		IF OBJECT_ID(N'dbo.sysdiagram_properties') IS NULL
		BEGIN
			CREATE TABLE dbo.sysdiagram_properties
			(
				diagram_id int,
				name sysname,
				value varbinary(max) NOT NULL
			)
		END
		*/

		IF OBJECT_ID(N'dbo.dtproperties') IS NOT NULL
		begin
			insert into dbo.sysdiagrams
			(
				[name],
				[principal_id],
				[version],
				[definition]
			)
			select	 
				convert(sysname, dgnm.[uvalue]),
				DATABASE_PRINCIPAL_ID(N'dbo'),			-- will change to the sid of sa
				0,							-- zero for old format, dgdef.[version],
				dgdef.[lvalue]
			from dbo.[dtproperties] dgnm
				inner join dbo.[dtproperties] dggd on dggd.[property] = 'DtgSchemaGUID' and dggd.[objectid] = dgnm.[objectid]	
				inner join dbo.[dtproperties] dgdef on dgdef.[property] = 'DtgSchemaDATA' and dgdef.[objectid] = dgnm.[objectid]
				
			where dgnm.[property] = 'DtgSchemaNAME' and dggd.[uvalue] like N'_EA3E6268-D998-11CE-9454-00AA00A3F36E_' 
			return 2;
		end
		return 1;
	END
	
GO
ALTER DATABASE [Ecoe] SET  READ_WRITE 
GO
