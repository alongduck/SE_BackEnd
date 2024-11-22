using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SkyModel;

public class NoLockInterceptor : DbCommandInterceptor
{
	private IEnumerable<string>? _ignoreTabbles;

	public NoLockInterceptor(IEnumerable<string>? ignoreTabbles = null) => _ignoreTabbles = ignoreTabbles;

	public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
		=> base.ReaderExecuting(command.NoLockCommand(_ignoreTabbles), eventData, result);

	public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
		=> base.ReaderExecutingAsync(command.NoLockCommand(_ignoreTabbles), eventData, result, cancellationToken);

	public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> interceptionContext)
		=> base.ScalarExecuting(command.NoLockCommand(_ignoreTabbles), eventData, interceptionContext);

	public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
		=> base.ScalarExecutingAsync(command.NoLockCommand(_ignoreTabbles), eventData, result, cancellationToken);

	public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
		=> base.NonQueryExecuting(command.NoLockCommand(_ignoreTabbles), eventData, result);

	public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
		=> base.NonQueryExecutingAsync(command.NoLockCommand(_ignoreTabbles), eventData, result, cancellationToken);
}

public static class DbCommandExtensions
{
	private static readonly Regex _tableAliasRegex = new(@"(?<table>((FROM)|(JOIN))\s[^\s]+\sAS\s[^\s]+\])", RegexOptions.Multiline | RegexOptions.IgnoreCase);

	public static DbCommand NoLockCommand(this DbCommand command, IEnumerable<string>? ignoreTables)
	{
		if (ignoreTables == null || !ignoreTables.Any())
			command.CommandText = _tableAliasRegex.Replace(command.CommandText, "${table}(NOLOCK)");
		else
		{
			MatchCollection matchs = _tableAliasRegex.Matches(command.CommandText);
			for (int i = matchs.Count - 1; i >= 0; i--)
			{
				string text = matchs[i].Value;
				string table = text.Remove(0, text.IndexOf('[') + 1);
				table = table.Remove(table.IndexOf(']'));
				if (!ignoreTables.Contains(table))
					command.CommandText = command.CommandText.Replace(text, text + "(NOLOCK)");
			}
		}
		return command;
	}
}