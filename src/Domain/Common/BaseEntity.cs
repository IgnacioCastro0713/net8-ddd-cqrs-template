﻿namespace Domain.Common;

public abstract class BaseEntity
{
	public int Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public string CreatedBy { get; set; } = default!;
	public DateTime UpdatedAt { get; set; }
	public string UpdatedBy { get; set; } = default!;
}