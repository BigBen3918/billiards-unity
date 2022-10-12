//  Author:
//  Salman Younas <salman.younas0007@gmail.com>
//
//  Copyright (c) 2018 Appic Studio

using System.Globalization;

public class Formatter {

	public const int NAME_MAX_LENGTH = 20;

	public static string FormatName(string name) {
		if (name.Length > NAME_MAX_LENGTH) {
			name = name.Substring (0, NAME_MAX_LENGTH);
		}

		return name;
	}

	public static string FormatCash(float amount) {
		if (amount > 999999999 || amount < -999999999) {
			return amount.ToString ("0,,,.###B", CultureInfo.InvariantCulture);
		}
		else if (amount > 999999 || amount < -999999) {
			return amount.ToString ("0,,.##M", CultureInfo.InvariantCulture);
		}
		else if (amount > 9999 || amount < -9999) {
			return amount.ToString ("0,.#K", CultureInfo.InvariantCulture);
		}
		else {
			return amount.ToString (CultureInfo.InvariantCulture);
		}
	}

}
