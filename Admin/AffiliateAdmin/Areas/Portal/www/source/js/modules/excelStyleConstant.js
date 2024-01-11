export const titleCellStyle = {
    font: {
        bold: true,
        color: {
            rgb: "333333"
        }
    },
    fill: {
        patternType: "solid",
        fgColor: {
            rgb: "cdd3d9"
        }
    }
}

export const headerCellStyle = {
    font: {
        bold: true,
        color: {
            rgb: "333333"
        },
        sz: "12"
    },
    alignment: {
        horizontal: "center",
        vertical: "center"
    },
    fill: {
        patternType: "solid",
        fgColor: {
            rgb: "e0e0e0"
        }
    },
    border: {
        left: {
            style: "thin",
            color: {
                rgb: "cccccc"
            }
        },
        right: {
            style: "thin",
            color: {
                rgb: "cccccc"
            }
        },
        top: {
            style: "thin",
            color: {
                rgb: "cccccc"
            }
        },
        bottom: {
            style: "thin",
            color: {
                rgb: "cccccc"
            }
        }
    }
}

export const mergedHeaderCellByRowspanStyle = {
    border: {
        left: {
            style: "thin",
            color: {
                rgb: "cccccc"
            }
        },
        right: {
            style: "thin",
            color: {
                rgb: "cccccc"
            }
        }
    }
}

export const mergedHeaderCellByColspanStyle = {
    border: {
        top: {
            style: "thin",
            color: {
                rgb: "cccccc"
            }
        },
        bottom: {
            style: "thin",
            color: {
                rgb: "cccccc"
            }
        }
    }
}

const bodyTextStyle = {
    alignment: {
        vertical: "center"
    }
};
const bodyDecimalStyle = {
    numFmt: "#,##0.00"
};
export const bodyCellStyle = {
    rowspan: bodyTextStyle,
    center: {
        alignment: {
            ...bodyTextStyle.alignment,
            horizontal: "center"
        }
    },
    left: {
        alignment: {
            ...bodyTextStyle.alignment,
            horizontal: "left"
        }
    },
    right: {
        alignment: {
            ...bodyTextStyle.alignment,
            horizontal: "right"
        }
    },
    number: {
        numFmt: "#,##0"
    },
    decimal: bodyDecimalStyle,
    negativeDecimal: {
        ...bodyDecimalStyle,
        font: {
            color: {
                rgb: "FF0000"
            }
        }
    },
    percent: {
        numFmt: '#,##0.00%'
    },
    rightHighlight: {
        font: {
            color: {
                rgb: "ff0000"
            }
        },
        alignment: {
            ...bodyTextStyle.alignment,
            horizontal: "right"
        }
    }
}

const totalTextStyle = {
    font: {
        bold: true
    },
    fill: {
        patternType: "solid",
        fgColor: {
            rgb: "ffc4a8"
        }
    }
};
const totalDecimalStyle = {
    ...totalTextStyle,
    numFmt: "#,##0.00"
};
export const totalCellStyle = {
    default: totalTextStyle,
    rowspan: {
        ...totalTextStyle,
        alignment: {
            horizontal: "center",
            vertical: "center"
        }
    },
    center: {
        ...totalTextStyle,
        alignment: {
            vertical: "center",
            horizontal: "center"
        }
    },
    right: {
        ...totalTextStyle,
        alignment: {
            vertical: "center",
            horizontal: "right"
        }
    },
    number: {
        ...totalTextStyle,
        numFmt: "#,##0"
    },
    decimal: totalDecimalStyle,
    negativeDecimal: {
        ...totalDecimalStyle,
        font: {
            bold: true,
            color: {
                rgb: "FF0000"
            }
        }
    }
}